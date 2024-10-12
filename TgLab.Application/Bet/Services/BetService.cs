﻿using TgLab.Application.Bet.DTOs;
using TgLab.Application.Bet.Interfaces;
using TgLab.Infrastructure.Context;
using TgLab.Application.User.Interfaces;
using BetDb = TgLab.Domain.Models.Bet;
using WalletDb = TgLab.Domain.Models.Wallet;
using TgLab.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace TgLab.Application.Bet.Services
{
    public class BetService : IBetService
    {
        private readonly TgLabContext _context;
        private readonly IUserService _userService;
        private readonly int MinBet = 1;

        public BetService(TgLabContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Create(CreateGambleDTO dto, string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.FirstOrDefault(w => w.Id == dto.WalletId);

            ArgumentNullException.ThrowIfNull(wallet);

            if (InvalidBet(dto, wallet))
            {
                throw new ArgumentException("Invalid bet amount");
            }
         
            var newBet = new BetDb()
            {
                WalletId = dto.WalletId,
                Amount = dto.Amount,
                Stage = BetStage.SENT,
                Bounty = 0,
                Time = DateTime.Now
            };

            _context.Bets.Add(newBet);
            _context.SaveChanges();

            await _userService.DecreaseUserBalance(user.Id, dto.WalletId, dto.Amount);
        }

        public async Task<IEnumerable<BetDTO>> ListBetsByWalletId(int walletId, string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.Id == walletId);
            
            ArgumentNullException.ThrowIfNull(wallet);

            return _context.Bets
                .Where(b => b.WalletId == walletId)
                .AsNoTracking()
                .Select(b => new BetDTO()
                {
                    Id = b.Id,
                    Currency = wallet.Currency,
                    Amount = b.Amount,
                    Stage = b.Stage,
                    Bounty = b.Bounty,
                    Time = b.Time
                })
                .ToList();
        }

        public async Task<IEnumerable<BetDTO>> ListAll(string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.UserId == user.Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var walletIds = user.Wallets.Select(w => w.Id);

            return _context.Bets
                .Where(b => walletIds.Contains(b.WalletId))
                .AsNoTracking()
                .Select(b => new BetDTO()
                {
                    Id = b.Id,
                    Currency = wallet.Currency,
                    Amount = b.Amount,
                    Stage = b.Stage,
                    Bounty = b.Bounty,
                    Time = b.Time
                })
                .ToList();
        }

        public async Task Cancel(int id, string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var walletIds = user.Wallets.Select(w => w.Id);

            var bet = _context.Bets.FirstOrDefault(b => b.Id == id);

            ArgumentNullException.ThrowIfNull(bet);

            if (!walletIds.Contains(bet.WalletId))
            {
                throw new ArgumentException($"[{Cancel}] Invalid bet");
            }

            bet.Stage = BetStage.CANCELLED;

            _context.Bets.Update(bet);
            _context.SaveChanges();
        }

        public bool InvalidBet(CreateGambleDTO bet, WalletDb wallet)
        {
            return bet.Amount < 0 || bet.Amount < MinBet || wallet.Balance < bet.Amount;
        }
    }
}

using TgLab.Infrastructure.Context;
using BetDb = TgLab.Domain.Models.Bet;
using WalletDb = TgLab.Domain.Models.Wallet;
using TgLab.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Game;
using TgLab.Domain.DTOs.Bet;
using TgLab.Domain.DTOs;
using TgLab.Domain.Interfaces.Bet;
using TgLab.Domain.Interfaces.User;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.Transaction;
using TgLab.Domain.DTOs.Transanction;
using TgLab.Domain.Exceptions.Bet;

namespace TgLab.Application.Bet.Services
{
    public class BetService : IBetService
    {
        private readonly TgLabContext _context;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;
        private readonly GameService _gameService;
        private readonly double MinBet = 1;

        public BetService(TgLabContext context, IUserService userService, IWalletService walletService, ITransactionService transactionService, GameService gameService)
        {
            _context = context;
            _userService = userService;
            _walletService = walletService;
            _transactionService = transactionService;
            _gameService = gameService;
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
                Stage = BetStage.SENT.Value,
                Bounty = 0,
                Time = DateTime.Now
            };

            var result = _context.Bets.Add(newBet).Entity;
            _context.SaveChanges();

            result.Wallet = wallet;

            await _walletService.DecreaseBalance(wallet, dto.Amount);

            var transaction = new CreateTransactionDTO()
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.BET
            };

            await _transactionService.Create(transaction);

            _gameService.DoBet(result);
        }

        public void Update(BetDb bet)
        {
            var betDb = _context.Bets
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == bet.Id);

            ArgumentNullException.ThrowIfNull(betDb);

            var result = _context.Bets.Update(bet);
            _context.SaveChanges();
        }

        public async Task<PaginatedList<BetDTO>> ListBetsByWalletId(int walletId, string userEmail, int pageIndex, int pageSize)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.Id == walletId);
            
            ArgumentNullException.ThrowIfNull(wallet);

            var bets = _context.Bets
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
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var count = await _context.Bets.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<BetDTO>(bets, pageIndex, totalPages);
        }

        public async Task<PaginatedList<BetDTO>> ListAll(string userEmail, int pageIndex, int pageSize)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var walletIds = user.Wallets.Select(w => w.Id);

            var bets = _context.Bets
                .Where(b => walletIds.Contains(b.WalletId))
                .Include(b => b.Wallet)
                .AsNoTracking()
                .Select(b => new BetDTO()
                {
                    Id = b.Id,
                    Currency = b.Wallet.Currency,
                    Amount = b.Amount,
                    Stage = b.Stage,
                    Bounty = b.Bounty,
                    Time = b.Time
                })
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var count = await _context.Bets.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<BetDTO>(bets, pageIndex, totalPages);
        }

        public async Task CancelBetOperation(int id, string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var walletIds = user.Wallets.Select(w => w.Id);

            var bet = _context.Bets
                .Include(b => b.Wallet)
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);

            ArgumentNullException.ThrowIfNull(bet);

            if (!walletIds.Contains(bet.WalletId))
            {
                throw new ArgumentException($"[{nameof(Cancel)}] Invalid bet");
            }

            var transaction = new CreateTransactionDTO()
            {
                WalletId = bet.Wallet.Id,
                Amount = bet.Amount,
                Type = TransactionType.REFUND
            };

            await Cancel(bet);
            await _transactionService.Create(transaction);
            await _walletService.IncreaseBalance(bet.Wallet, bet.Amount);
        }

        public async Task Cancel(BetDb bet)
        {
            if (bet.Stage == BetStage.CANCELLED.Value)
            {
                throw new AllReadyCancelled();
            }

            bet.Stage = BetStage.CANCELLED.Value;

            _context.Bets.Update(bet);
            _context.SaveChanges();
        }

        public bool InvalidBet(CreateGambleDTO bet, WalletDb wallet)
        {
            return bet.Amount < 0 || bet.Amount < MinBet || wallet.Balance < bet.Amount;
        }

        public bool IsCancelled(int id)
        {
            var bet = _context.Bets
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);
            
            ArgumentNullException.ThrowIfNull(bet);

            return bet.Stage == BetStage.CANCELLED.Value;
        }
    }
}

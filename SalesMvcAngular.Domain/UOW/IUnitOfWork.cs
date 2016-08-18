using System;
using SalesMvcAngular.Domain.Repositories;

namespace SalesMvcAngular.Domain.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<Customer> CustomersRepository { get; }
        Repository<InvoiceDetail> InvoiceDetailsRepository { get; }
        Repository<InvoiceHeader> InvoiceHeaderRepository { get; }
        Repository<Item> ItemRepository { get; }
        Repository<Store> StoreRepository { get; }
        Repository<StoreTransaction> StoreTransactionRepository { get; }
        Repository<Supplier> SupplierRepository { get; }
        Repository<User> UserRepository { get; }
        bool Save();
        void Dispose();
    }
}
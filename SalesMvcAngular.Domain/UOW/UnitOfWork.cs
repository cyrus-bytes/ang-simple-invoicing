using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesMvcAngular.Domain.Repositories;

namespace SalesMvcAngular.Domain.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private SalesDBContext context = new SalesDBContext();
        private Repository<Customer> customersRepository;
        private Repository<InvoiceDetail> invoiceDetailsRepository;
        private Repository<InvoiceHeader> invoiceHeaderRepository;
        private Repository<Item> itemRepository;
        private Repository<Store> storeRepository;
        private Repository<StoreTransaction> storeTransactionRepository;
        private Repository<Supplier> supplierRepository;
        private Repository<User> usersRepository;



        public Repository<Customer> CustomersRepository
        {
            get
            {

                if (this.customersRepository == null)
                {
                    this.customersRepository = new Repository<Customer>(context);
                }
                return customersRepository;
            }
        }

        public Repository<InvoiceDetail> InvoiceDetailsRepository
        {
            get
            {

                if (this.invoiceDetailsRepository == null)
                {
                    this.invoiceDetailsRepository = new Repository<InvoiceDetail>(context);
                }
                return invoiceDetailsRepository;
            }
        }

        public Repository<InvoiceHeader> InvoiceHeaderRepository
        {
            get
            {
                if (this.invoiceHeaderRepository == null)
                {
                    this.invoiceHeaderRepository = new Repository<InvoiceHeader>(context);
                }
                return invoiceHeaderRepository;
            }
        }

        public Repository<Item> ItemRepository
        {
            get
            {
                if (this.itemRepository == null)
                {
                    this.itemRepository=new Repository<Item>(context);
                }
                return itemRepository;
            }
        }

        public Repository<Store> StoreRepository
        {
            get
            {
                if (this.storeRepository==null)
                {
                    this.storeRepository = new Repository<Store>(context);
                }
            return storeRepository;
            }
        }

        public Repository<StoreTransaction> StoreTransactionRepository
        {
            get
            {
                if (this.storeTransactionRepository == null)
                {
                    this.storeTransactionRepository = new Repository<StoreTransaction>(context);
                }
                return storeTransactionRepository;
            }
        }

        public Repository<Supplier> SupplierRepository
        {
            get
            {
                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new Repository<Supplier>(context);
                }
                return supplierRepository;
            }
        }

        public Repository<User> UserRepository
        {
            get
            {
                if (this.usersRepository == null)
                {
                    this.usersRepository = new Repository<User>(context);
                }
                return usersRepository;
            }
        }
        public bool Save()
        {
            bool isSaved = false;
            try
            {
                if (context.SaveChanges() > 0)
                {
                    isSaved = true;
                }
            }
            catch
            {               
            }
            return isSaved;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

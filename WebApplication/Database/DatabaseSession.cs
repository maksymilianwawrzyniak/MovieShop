using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace WebApplication.Database
{
    public class DatabaseSession : IDisposable
    {
        private readonly IAsyncSession _session;

        private DatabaseSession(IDriver driver, string databaseName)
        {
            _session = driver.AsyncSession(x => x.WithDatabase(databaseName));
        }
        
        public static DatabaseSession StartSession(IDriver driver, string databaseName)
        {
            return new DatabaseSession(driver, databaseName);
        }

        public async Task<IResultCursor> RunAsync(string query)
        {
            return await _session.RunAsync(query);
        }

        public async Task<IResultCursor> RunAsync(string query, object parameters)
        {
            return await _session.RunAsync(query, parameters);
        }

        public async Task<IResultCursor> RunAsync(string query, IDictionary<string, object> parameters)
        {
            return await _session.RunAsync(query, parameters);
        }

        public async Task<IResultCursor> RunAsync(Query query)
        {
            return await _session.RunAsync(query);
        }

        public async Task<IAsyncTransaction> BeginTransactionAsync()
        {
            return await _session.BeginTransactionAsync();
        }

        public async Task<IAsyncTransaction> BeginTransactionAsync(Action<TransactionConfigBuilder> action)
        {
            return await _session.BeginTransactionAsync(action);
        }

        public async Task<T> ReadTransactionAsync<T>(Func<IAsyncTransaction, Task<T>> work)
        {
            return await _session.ReadTransactionAsync(work);
        }

        public async Task ReadTransactionAsync(Func<IAsyncTransaction, Task> work)
        {
            await _session.ReadTransactionAsync(work);
        }

        public async Task<T> ReadTransactionAsync<T>(Func<IAsyncTransaction, Task<T>> work, Action<TransactionConfigBuilder> action)
        {
            return await _session.ReadTransactionAsync(work, action);
        }

        public async Task ReadTransactionAsync(Func<IAsyncTransaction, Task> work, Action<TransactionConfigBuilder> action)
        {
            await _session.ReadTransactionAsync(work, action);
        }

        public async Task<T> WriteTransactionAsync<T>(Func<IAsyncTransaction, Task<T>> work)
        {
            return await _session.WriteTransactionAsync(work);
        }

        public async Task WriteTransactionAsync(Func<IAsyncTransaction, Task> work)
        {
            await _session.WriteTransactionAsync(work);
        }

        public async Task<T> WriteTransactionAsync<T>(Func<IAsyncTransaction, Task<T>> work, Action<TransactionConfigBuilder> action)
        {
            return await _session.WriteTransactionAsync(work, action);
        }

        public async Task WriteTransactionAsync(Func<IAsyncTransaction, Task> work, Action<TransactionConfigBuilder> action)
        {
            await _session.WriteTransactionAsync(work, action);
        }

        public async Task CloseAsync()
        {
            await _session.CloseAsync();
        }

        public async Task<IResultCursor> RunAsync(string query, Action<TransactionConfigBuilder> action)
        {
            return await _session.RunAsync(query, action);
        }

        public async Task<IResultCursor> RunAsync(string query, IDictionary<string, object> parameters, Action<TransactionConfigBuilder> action)
        {
            return await _session.RunAsync(query, parameters, action);
        }

        public async Task<IResultCursor> RunAsync(Query query, Action<TransactionConfigBuilder> action)
        {
            return await _session.RunAsync(query, action);
        }

        public Bookmark LastBookmark => _session.LastBookmark;

        public async void Dispose()
        {
            await CloseAsync();
        }
    }
}
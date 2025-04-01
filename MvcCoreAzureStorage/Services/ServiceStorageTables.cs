using Azure.Data.Tables;
using MvcCoreAzureStorage.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageTables
    {
        private TableClient tableClient;

        public ServiceStorageTables(TableServiceClient tableService)
        {
            this.tableClient = tableService.GetTableClient("clientes");
        }

        public async Task CreateClientAsync(int id, string nombre, string empresa, int salario, int edad)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Empresa = empresa,
                Nombre = nombre,
                Salario = salario,
                Edad = edad
            };
            await this.tableClient.AddEntityAsync<Cliente>(cliente);
        }

        public async Task<Cliente> FindClientAsync(string partitionKey, string rowKey)
        {
            Cliente cliente = await this.tableClient.GetEntityAsync<Cliente>(partitionKey, rowKey);
            return cliente;
        }

        public async Task DeleteClientAsync(string partitionKey, string rowKey)
        {
            await this.tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<List<Cliente>> GetClientAsync()
        {
            List<Cliente> clientes = new List<Cliente>();
            //PARA BUSCAR NECESITAMOS UTILIZAR UN OBJETO QUERY
            //CON UN FILTER
            var query = this.tableClient.QueryAsync<Cliente>(filter: "");
            //DEBEMOS EXTRAR TODOS LOS DATOS DEL QUERY
            await foreach(var item in query)
            {
                clientes.Add(item);
            }
            return clientes;
        }

        public async Task<List<Cliente>> GetClientesEmpresaAsync(string empresa)
        {
            //TENEMOS DOS TIPOS DE FILTER, LOS DOS SE UTILIZAN CON query
            //1)SI REALIZAMOS EL FILTER CON QueryAsync,
            //DEBEMOS UTILIZAR UNA SINTAXIS Y EXTRAER LOS DATOS MANUALES
            //string filtro = "Campo eq valor";
            //string filtro = "Campo eq valor and Campo2 gt valor2";
            //string filtro = "Campo lt valor and Campo2 gt valor2";
            //string filtroSalario = "Salario gt 250000 and Salario lt 300000";
            //var query = this.tableClient.QueryAsync<Cliente>(filter: filtroSalario);

            //2) SI REALIZAMOS LA CONSULTA CON Query
            //PODEMOS UTILIZAR LAMBDA Y EXTRAER LOS DATOS DIRECTAMENTE,
            //PERO NO ES ASINCRONO
            var query = this.tableClient.Query<Cliente>(x => x.Empresa == empresa);
            return query.ToList();
        }
    }
}

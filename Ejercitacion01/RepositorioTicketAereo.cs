using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ejercitacion01
{
   public class RepositorioTicketAereo
    {
        private static RepositorioTicketAereo instancia;
        private IConfigurationRoot configuration;
        List<TicketAereo> tickets;


        public RepositorioTicketAereo()
        {
            tickets = new List<TicketAereo>();
            RecuperarTickets();
        }

        public IReadOnlyCollection<TicketAereo> ListarTicketsAereos()
        {
            return tickets.AsReadOnly();
        }
        public static RepositorioTicketAereo Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new RepositorioTicketAereo();
                }
                return instancia;
            }
        }
        public bool Agregar(TicketAereo ticket)
        {
            var ok = false;
            var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var sqlTransaction = connection.BeginTransaction();
             try
            {
                using (var command = new SqlCommand())
                { 
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_AgregarTicket";
                command.Connection = connection;
                command.Transaction = sqlTransaction;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Origen;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Destino;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 15).Value = ticket.pasajero.NroPasaporte;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 6).Value = ticket.avion.Matricula;
                command.Parameters.Add("", System.Data.SqlDbType.DateTime).Value = ticket.FechaDeVuelo;
                command.ExecuteNonQuery();
                sqlTransaction.Commit();
                connection.Close();
                tickets.Add(ticket);
                ok = true;
            }
            }

            catch (SqlException ex) 
            {
                sqlTransaction.Rollback();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex) 
            {
                sqlTransaction.Rollback();
                connection.Close();
                connection.Dispose();
            }
            return ok;
        }

        public bool Eliminar(TicketAereo ticket) 
        {
            var ok = false;
            var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var sqlTransaction = connection.BeginTransaction();
            try
            {
                using (var command = new SqlCommand())
                { 
                    command.CommandText = "SP_EliminarTicket";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Connection = connection;
                command.Transaction = sqlTransaction;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 25).Value = ticket.NroTicket;
                command.ExecuteNonQuery();
                sqlTransaction.Commit();
                connection.Close();
                tickets.Remove(ticket);
                ok = true;
            }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                connection.Close();
                connection.Dispose();
            }
            return ok;
        }

        public bool Modificar(TicketAereo ticket)
        {
            var ok = false;
            var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var sqlTransaction = connection.BeginTransaction();

            try
            {
                var command = new SqlCommand();

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_ModificarTicket";
                command.Connection = connection;
                command.Transaction = sqlTransaction;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Origen;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 50).Value = ticket.Destino;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 15).Value = ticket.pasajero.NroPasaporte;
                command.Parameters.Add("", System.Data.SqlDbType.NVarChar, 6).Value = ticket.avion.Matricula;
                command.Parameters.Add("", System.Data.SqlDbType.DateTime).Value = ticket.FechaDeVuelo;
                command.ExecuteNonQuery();
                sqlTransaction.Commit();
                connection.Close();
                tickets.Add(ticket);
                ok = true;
            }
            catch (SqlException ex)
            {
                sqlTransaction.Rollback();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                connection.Close();
                connection.Dispose();
            }
            return ok;
        }

        public void RecuperarTickets()
        {
            using (var connection = new SqlConnection(""))

                try
                {
                    var command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "";

                    command.Connection = connection;
                    command.Connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticket = new TicketAereo();
                        ticket.avion.Matricula = reader[""].ToString();
                        ticket.Origen = reader[""].ToString();
                        ticket.Destino = reader[""].ToString();
                        ticket.pasajero.NroPasaporte = reader[""].ToString() ;
                        ticket.avion.Matricula = reader[""].ToString().ToUpper();
                        ticket.FechaDeVuelo = Convert.ToDateTime(reader[""].ToString());


                        tickets.Add(ticket);
                    }
                    command.Connection.Close();
                }
                catch (SqlException ex)
                {
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                }
        }
    }
}

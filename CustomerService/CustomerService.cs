using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace CustomerService
{
    public class CustomerService
    {
        protected string GetDBconnectionstring()
        {
            return ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;
        }
        private List<CustomerModel.Customer> MapCustomerDataToList(DataTable dt)
        {
            List<CustomerModel.Customer> result = new List<CustomerModel.Customer>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new CustomerModel.Customer()
                {
                    CustomerID = Convert.ToString(row["CustomerID"]),
                    CompanyName = (string)row["CompanyName"],
                    ContactName = (string)row["ContactName"],
                    ContactTitle = (string)row["Codeval"]
                });
            }
            return result;
        }
        public List<CustomerModel.Customer> GetCustomer(CustomerModel.Customer customer)
        {
            DataTable dt = new DataTable();
            string SQL = @"Select CustomerId,CompanyName,ContactName,Codeval from Sales.Customers a join dbo.CodeTable b on a.ContactTitle=b.CodeId where CustomerID like @CustomerID and
                            CompanyName like @CompanyName and ContactName like @ContactName and ContactTitle like @ContactTitle and CodeType='TITLE'";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", customer.CustomerID == null? "%%" : "%" + Convert.ToString(customer.CustomerID) + "%"));
                    cmd.Parameters.Add(new SqlParameter("@CompanyName",  customer.CompanyName== null ? "%%" : "%" + Convert.ToString(customer.CompanyName) + "%"));
                    cmd.Parameters.Add(new SqlParameter("@ContactName", customer.ContactName == null ? "%%" : Convert.ToString(customer.ContactName)));
                    cmd.Parameters.Add(new SqlParameter("@ContactTitle", customer.ContactTitle == null ? "%%" : Convert.ToString(customer.ContactTitle)));
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return this.MapCustomerDataToList(dt);
        }
        public List<SelectListItem> GetContactTitle()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select CodeId,CodeVal from CodeTable where CodeType='Title'";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[1].ToString(), Value = rd[0].ToString() });
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
                return list;
            }
        }
        public int InsertCustomer(CustomerModel.Customer customer)
        {
            string SQL = @"Insert into Sales.Customers values(
                         @CompanyName,@ContactName,@ContactTitle,@CreationDate,@Address,@City,
                         @Region,@PostalCode,@Country,@Phone,@Fax)
                         Select Cast(SCOPE_IDENTITY() as int)";
            int id = 0;
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", customer.CompanyName));
                    cmd.Parameters.Add(new SqlParameter("@ContactName", customer.ContactName));
                    cmd.Parameters.Add(new SqlParameter("@ContactTitle", customer.ContactTitle));
                    cmd.Parameters.Add(new SqlParameter("@CreationDate", string.Format("{0:yyyy/MM/dd}", customer.CreationDate)));
                    cmd.Parameters.Add(new SqlParameter("@Address", customer.Address));
                    cmd.Parameters.Add(new SqlParameter("@City", customer.City));
                    cmd.Parameters.Add(new SqlParameter("@Region", customer.Region));
                    cmd.Parameters.Add(new SqlParameter("@PostalCode", customer.PostalCode));
                    cmd.Parameters.Add(new SqlParameter("@Country", customer.Country));
                    cmd.Parameters.Add(new SqlParameter("@Phone", customer.Phone));
                    cmd.Parameters.Add(new SqlParameter("@Fax", customer.Fax));
                    id = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return id;
        }
    }
}

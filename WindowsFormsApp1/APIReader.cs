
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace WebApplicationHospital
{
    public class Patient
    {
        public int Id_Patient { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string PassportData { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }

        public Patient() { }

        public Patient(object[] patientData)
        {
            Id_Patient = Convert.ToInt32(patientData[0]);
            LastName = Convert.ToString(patientData[1]);
            FirstName = Convert.ToString(patientData[2]);
            Patronymic = Convert.ToString(patientData[3]);
            PassportData = Convert.ToString(patientData[4]);
            DateOfBirth = Convert.ToString(patientData[5]);
            Gender = Convert.ToString(patientData[6]);
            Address = Convert.ToString(patientData[7]);
            PhoneNumber = Convert.ToString(patientData[8]);
            Email = Convert.ToString(patientData[9]);

            if (patientData[10] != DBNull.Value)
            {
                Photo = (byte[])patientData[10];
            }
        }

    }

    internal class APIReader
    {
        private static readonly string url = "http://172.20.10.7:5181";
        private static readonly HttpClient client = SettingHttpClient();

        public static HttpClient SettingHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            HttpClient client = new HttpClient(handler);
            return client;
        }

        public static async Task<Patient> getPatient(int id)
        {
            if (!await canConnectToAPI())
                return null;

            Patient patient = null;
            HttpResponseMessage response = await client.GetAsync(url + $"/Patient/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                patient = await response.Content.ReadFromJsonAsync<Patient>();
            }
            return patient;
        }

        public static async Task<List<Patient>> getPatients()
        {
            if (!await canConnectToAPI())
                return null;

            List<Patient> patients = null;
            HttpResponseMessage response = await client.GetAsync(url + "/Patient");
            if (response.IsSuccessStatusCode)
            {
                patients = await response.Content.ReadFromJsonAsync<List<Patient>>();
            }
            return patients;
        }

        public static async Task<bool> canConnectToAPI()
        {
            try
            {
                await client.GetAsync(url);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

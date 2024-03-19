using HospitalApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApplicationHospital;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string imagePath = null;

            if (pictureBox2.Image != null)
            {
                // Задаем путь для сохранения изображения
                string tempPath = Path.GetTempPath();

                // Генерируем уникальное имя файла
                string fileName = Path.GetRandomFileName();
                imagePath = Path.Combine(tempPath, fileName + ".png");

                // Сохраняем изображение во временной папке
                pictureBox2.Image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);

                Console.WriteLine("Изображение сохранено во временной папке: " + imagePath);
            }
            else
            {
                Console.WriteLine("В PictureBox нет изображения для сохранения.");
            }

            if (imagePath != null)
            {
                ArrayList Parameters = new ArrayList
            {
            "@PatientID", 2,
            "@PhotoPath", imagePath ,
            };

                DataBaseWorker.ExecuteStoredProcedure("InsertPatientPhoto", Parameters);



                List<object[]> responsePatients = DataBaseWorker.ExecuteQueryObject($"SELECT * FROM Patients WHERE Id_Patient = 2", 11); ;
                Patient p = new Patient(responsePatients[0]);

                using (MemoryStream ms = new MemoryStream(p.Photo))
                {
                    Image image = Image.FromStream(ms);
                    pictureBox1.Image = image;
                }
            }

            try
            {
                File.Delete(imagePath);
                Console.WriteLine("Файл успешно удален.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
            }
        }
    }
}

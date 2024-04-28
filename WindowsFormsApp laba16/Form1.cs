using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_laba13
{
    public partial class Form1 : Form
    {
        private List<AircraftData> aircraftList = new List<AircraftData>();
        [Serializable]
        public class AircraftData
        {
            public string Model { get; set; }
            public string Manufacturer { get; set; }
            public string Type { get; set; }
            public int PassengerCapacity { get; set; }
            public double FuelConsumption { get; set; }
            public int MaxSpeed { get; set; }
            public bool HasAutopilot { get; set; }
            public bool NoAutopilot { get; set; }
        }
        public Form1()
        {
            {

                InitializeComponent();

                // Відключаємо автоматичне створення стовпців
                dataGridView1.AutoGenerateColumns = false;

                // Додаємо стовпці вручну
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Модель";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Manufacturer";
                column.Name = "Виробник";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Тип літака";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "PassengerCapacity";
                column.Name = "Пасажиро стійкість";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "FuelConsumption";
                column.Name = "Споживання пального";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "MaxSpeed";
                column.Name = "Макс швидкість";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewCheckBoxColumn();
                column.DataPropertyName = "HasAutopilot";
                column.Name = "Є автопілот";
                dataGridView1.Columns.Add(column);

                column = new DataGridViewCheckBoxColumn();
                column.DataPropertyName = "NoAutopilot";
                column.Name = "Нема автопілота";
                dataGridView1.Columns.Add(column);
               
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK) // Відображаємо Form2 як модальне вікно і чекаємо, доки воно не буде закрите
            {
                // Якщо користувач натиснув OK на формі Form2, то передаємо дані до DataGridView
                dataGridView1.Rows.Add(form2.Model, form2.Manufacturer, form2.Type, form2.PassengerCapacity, form2.FuelConsumption, form2.MaxSpeed, form2.HasAutopilot, form2.NoAutopilot);
            }
        }
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            // Видаляємо виділену лінію у dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
        }
        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            // Перевірка, чи є виділені комірки
            if (dataGridView1.SelectedCells.Count > 0)
            {
                // Вмикаємо кнопку 3
                toolStripButton3.Checked = false;
            }
        }
        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            // Очищаємо dataGridView1
            dataGridView1.Rows.Clear();
        }
        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // Оновлення aircraftList перед збереженням у текстовий файл
            RefreshAircraftList();

            // Збереження даних у текстовий файл
            using (StreamWriter writer = new StreamWriter("aircraft_data.txt"))
            {
                foreach (AircraftData aircraft in aircraftList)
                {
                    // Перевірка на повне заповнення об'єкта AircraftData перед збереженням
                    if (IsAircraftDataFullyFilled(aircraft))
                    {
                        writer.WriteLine($"Model: {aircraft.Model}");
                        writer.WriteLine($"Manufacturer: {aircraft.Manufacturer}");
                        writer.WriteLine($"Type: {aircraft.Type}");
                        writer.WriteLine($"Passenger Capacity: {aircraft.PassengerCapacity}");
                        writer.WriteLine($"Fuel Consumption: {aircraft.FuelConsumption}");
                        writer.WriteLine($"Max Speed: {aircraft.MaxSpeed}");
                        writer.WriteLine($"Has Autopilot: {aircraft.HasAutopilot}");
                        writer.WriteLine($"No Autopilot: {aircraft.NoAutopilot}");
                        writer.WriteLine();
                    }
                }
            }
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
                aircraftList.Clear();
                using (StreamReader reader = new StreamReader("aircraft_data.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        AircraftData aircraft = new AircraftData();
                        aircraft.Model = reader.ReadLine()?.Split(':')[1].Trim();
                        aircraft.Manufacturer = reader.ReadLine()?.Split(':')[1].Trim();
                        aircraft.Type = reader.ReadLine()?.Split(':')[1].Trim();
                        aircraft.PassengerCapacity = int.Parse(reader.ReadLine()?.Split(':')[1].Trim());
                        aircraft.FuelConsumption = double.Parse(reader.ReadLine()?.Split(':')[1].Trim());
                        aircraft.MaxSpeed = int.Parse(reader.ReadLine()?.Split(':')[1].Trim());
                        aircraft.HasAutopilot = bool.Parse(reader.ReadLine()?.Split(':')[1].Trim());
                        aircraft.NoAutopilot = bool.Parse(reader.ReadLine()?.Split(':')[1].Trim());
                        reader.ReadLine(); 
                        aircraftList.Add(aircraft);
                    }
                }
                RefreshDataGridView();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            // Оновлення aircraftList перед збереженням у бінарний файл
            RefreshAircraftList();

            // Збереження даних у бінарний файл
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("aircraft_data.bin", FileMode.Create))
            {
                // Створюємо тимчасовий список для зберігання лише повністю заповнених об'єктів AircraftData
                List<AircraftData> fullyFilledAircraftList = aircraftList.Where(IsAircraftDataFullyFilled).ToList();

                // Серіалізуємо тільки ті об'єкти, які мають всі поля заповнені
                formatter.Serialize(stream, fullyFilledAircraftList);
            }
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("aircraft_data.bin", FileMode.Open))
            {
                aircraftList = (List<AircraftData>)formatter.Deserialize(stream);
            }

            // Оновлення DataGridView після завантаження даних
            RefreshDataGridView();
        }
        private void RefreshDataGridView()
        {
            dataGridView1.Rows.Clear();
            foreach (AircraftData aircraft in aircraftList)
            {
                dataGridView1.Rows.Add(aircraft.Model, aircraft.Manufacturer, aircraft.Type, aircraft.PassengerCapacity,
                                        aircraft.FuelConsumption, aircraft.MaxSpeed, aircraft.HasAutopilot, aircraft.NoAutopilot);
            }
        }
        private void RefreshAircraftList()
        {
            aircraftList.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Створення нового об'єкту AircraftData з даних у dataGridView1 і додавання його до списку aircraftList
                AircraftData aircraft = new AircraftData();
                aircraft.Model = row.Cells["Модель"].Value?.ToString() ?? ""; // перевірка на null і, якщо значення null, призначення порожньої строки
                aircraft.Manufacturer = row.Cells["Виробник"].Value?.ToString() ?? "";
                aircraft.Type = row.Cells["Тип літака"].Value?.ToString() ?? "";
                aircraft.PassengerCapacity = row.Cells["Пасажиро стійкість"].Value != null ? int.Parse(row.Cells["Пасажиро стійкість"].Value.ToString()) : 0; // перевірка на null перед конвертацією в int
                aircraft.FuelConsumption = row.Cells["Споживання пального"].Value != null ? double.Parse(row.Cells["Споживання пального"].Value.ToString()) : 0.0; // перевірка на null перед конвертацією в double
                aircraft.MaxSpeed = row.Cells["Макс швидкість"].Value != null ? int.Parse(row.Cells["Макс швидкість"].Value.ToString()) : 0; // перевірка на null перед конвертацією в int
                aircraft.HasAutopilot = row.Cells["Є автопілот"].Value != null && (bool)row.Cells["Є автопілот"].Value; // перевірка на null перед зверненням до значення клітинки
                aircraft.NoAutopilot = row.Cells["Нема автопілота"].Value != null && (bool)row.Cells["Нема автопілота"].Value; // перевірка на null перед зверненням до значення клітинки
                aircraftList.Add(aircraft);
            }
        }
        private bool IsAircraftDataFullyFilled(AircraftData aircraft)
        {
            return !string.IsNullOrWhiteSpace(aircraft.Model) &&
                   !string.IsNullOrWhiteSpace(aircraft.Manufacturer) &&
                   !string.IsNullOrWhiteSpace(aircraft.Type) &&
                   aircraft.PassengerCapacity != 0 &&
                   aircraft.FuelConsumption != 0.0 &&
                   aircraft.MaxSpeed != 0;
        }
    }
}


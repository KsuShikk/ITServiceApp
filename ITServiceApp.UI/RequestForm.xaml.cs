using ITServiceApp.Domain.Models;
using System.Linq;
using System.Windows;

namespace ITServiceApp.UI
{
    public partial class RequestForm : Window
    {
        public ServiceRequest Request { get; private set; }

        public RequestForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        public RequestForm(ServiceRequest request) : this()
        {
            Request = request;
            FillForm();
        }

        private void InitializeForm()
        {
            // Заполняем комбобокс типами оборудования
            string[] equipmentTypes = { "Компьютер", "Сервер", "Принтер", "Монитор", "Ноутбук", "Сетевое оборудование" };
            foreach (var type in equipmentTypes)
            {
                EquipmentTypeComboBox.Items.Add(type);
            }

            if (Request == null)
            {
                RequestNumberTextBox.Text = GenerateRequestNumber();
            }
        }

        private void FillForm()
        {
            if (Request != null)
            {
                RequestNumberTextBox.Text = Request.RequestNumber;
                EquipmentTypeComboBox.SelectedItem = Request.EquipmentType;
                EquipmentModelTextBox.Text = Request.EquipmentModel;
                ProblemDescriptionTextBox.Text = Request.ProblemDescription;
                ClientNameTextBox.Text = Request.ClientName;
                PhoneNumberTextBox.Text = Request.PhoneNumber;
            }
        }

        private string GenerateRequestNumber()
        {
            return $"REQ{System.DateTime.Now:yyyyMMddHHmmss}";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                if (Request == null)
                {
                    Request = new ServiceRequest(
                        RequestNumberTextBox.Text,
                        EquipmentTypeComboBox.SelectedItem.ToString(),
                        EquipmentModelTextBox.Text,
                        ProblemDescriptionTextBox.Text,
                        ClientNameTextBox.Text,
                        PhoneNumberTextBox.Text
                    );
                }
                else
                {
                    Request.EquipmentType = EquipmentTypeComboBox.SelectedItem.ToString();
                    Request.EquipmentModel = EquipmentModelTextBox.Text;
                    Request.ProblemDescription = ProblemDescriptionTextBox.Text;
                    Request.ClientName = ClientNameTextBox.Text;
                    Request.PhoneNumber = PhoneNumberTextBox.Text;
                }

                DialogResult = true;
                Close();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(RequestNumberTextBox.Text))
            {
                MessageBox.Show("Введите номер заявки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (EquipmentTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип оборудования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(EquipmentModelTextBox.Text))
            {
                MessageBox.Show("Введите модель оборудования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ClientNameTextBox.Text))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

﻿using ContactsApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactsApp.View
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Поле для хранения объектов контактов.
        /// </summary>
        private Project _project { get; set; }

        /// <summary>
        /// Конструктор мейн формы.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _project = new Project();
            _project = ProjectManager.LoadFromFile();
            UpdateListBox();
        }

        /// <summary>
        /// Обновляет данные в лист боксе.
        /// </summary>
        private void UpdateListBox()
        {
            ContactsListBox.Items.Clear();
            _project.Contacts = _project.SortContacts();
            foreach (var contact in _project.Contacts)
            {
                ContactsListBox.Items.Add(contact);
            }
        }

        /// <summary>
        /// Добавляет контакт с выбранными полями.
        /// </summary>
        private void AddContact()
        {
            var contactForm = new ContactForm();
            var result = contactForm.ShowDialog();
            var newContact = contactForm.Contact;
            if (result == DialogResult.OK)
            {
                _project.Contacts.Add(newContact);
            }
            ProjectManager.SaveToFile(_project);
            ClearSelectedContact();
        }

        /// <summary>
        /// Редактирование контакта.
        /// </summary>
        private void EditContact()
        {
            var contactForm = new ContactForm();
            contactForm.Contact = GetEditContact(ContactsListBox.SelectedIndex);
            var result = contactForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                _project.Contacts[ContactsListBox.SelectedIndex] = contactForm.Contact;
            }
            ProjectManager.SaveToFile(_project);
            ClearSelectedContact();
        }

        /// <summary>
        /// Удаляет выбранный контакт по индексу.
        /// </summary>
        /// <param name="index"></param>
        private void RemoveContact(int index)
        {
            if (index == -1 || ContactsListBox.Items.Count == 0)
            {
                return;
            }

            var result = MessageBox.Show($"Do you really want to remove {_project.Contacts[index].Surname}?",
                "Warning", MessageBoxButtons.OKCancel);
            
            if (result == DialogResult.Cancel)
            {
                return;
            }

            _project.Contacts.RemoveAt(index);
            ProjectManager.SaveToFile(_project);
            ClearSelectedContact();
        }

        /// <summary>
        /// Обновляет выбранный контакт по индексу.
        /// </summary>
        /// <param name="index">Индекс контакта</param>
        private void UpdateSelectedContact(Contact contact)
        {
            if (contact == null)
            {
                return;
            }

            SurnameTextBox.Text = contact.Surname;
            NameTextBox.Text = contact.Name;
            PhoneTextBox.Text = contact.PhoneNumber.Number.ToString();
            BirthdayDateTimePicker.Value = contact.DateOfBirth;
            VkTextBox.Text = contact.VkId;
            EmailTextBox.Text = contact.Email;
        }

        /// <summary>
        /// Удаляет выбранный контакт.
        /// </summary>
        private void ClearSelectedContact()
        {
            SurnameTextBox.Text = String.Empty;
            NameTextBox.Text = String.Empty;
            PhoneTextBox.Text = String.Empty;
            BirthdayDateTimePicker.Text = String.Empty;
            VkTextBox.Text = String.Empty;
            EmailTextBox.Text = String.Empty;
        }

        /// <summary>
        /// Получение копии контакта по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Contact GetEditContact(int index)
        {
            return (Contact)_project.Contacts[index].Clone();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddContact();
            UpdateListBox();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure want to exit?", 
                "Warning",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            ProjectManager.SaveToFile(_project);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            EditContact();
            UpdateListBox();
        }

        private void addContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddContact();
            UpdateListBox();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            RemoveContact(ContactsListBox.SelectedIndex);
            UpdateListBox();
        }

        private void removeContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveContact(ContactsListBox.SelectedIndex);
            UpdateListBox();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ContactslistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ContactsListBox.SelectedIndex == -1)
            {
                ClearSelectedContact();
                return;
            }
            UpdateSelectedContact((Contact)ContactsListBox.SelectedItem);
        }

        private void ClearSearchButton_Click(object sender, EventArgs e)
        {
            UpdateListBox();
            ClearSelectedContact();
        }

        /// <summary>
        /// Поиск по фамилии при вводе текста в текстбокс поиска.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ContactsListBox.Items.Clear();
            _project.Contacts = _project.SortContacts();
            foreach (var contact in _project.Contacts)
            {
                if (contact.Surname.Contains(SearchTextBox.Text))
                {
                    ContactsListBox.Items.Add(contact);
                }
            }
        }

        private void editContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditContact();
            UpdateListBox();
        }
    }
}

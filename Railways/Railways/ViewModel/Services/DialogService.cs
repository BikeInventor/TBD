using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.ViewModel;
using Railways.View;
using MaterialDesignThemes.Wpf;

namespace Railways.ViewModel.Services
{
    /// <summary>
    /// Служба, необходимая для отображения диалоговых окон
    /// Для возможности отображения диалогового окна,
    /// окно-родитель должно быть обёрнуто в DialogHost 
    /// с идентификатором, передающимся в ShowDialog()
    /// </summary>
    public static class DialogService
    {
        /// <summary>
        /// Отображение диалогового окна
        /// </summary>
        /// <param name="hostWindow">Родительское окно, из которого запускается диалоговое окно</param>
        /// <param name="message">Сообщение в диалоговом окне</param>
        /// <param name="dialogType">тип диалогового окна (информационное, окно выбора)</param>
        /// <returns></returns>
        public static async Task<bool> ShowDialog(String hostWindow, String message, DialogWindowType dialogType)
        {
            var context = new DialogViewModel(message, dialogType);
            var view = new DialogWindow()
            {
                DataContext = context
            };

            var result = (bool)await DialogHost.Show(view, hostWindow);

            return result;
        }
    }
}

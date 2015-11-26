using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Railways.Model.Logic;
using System.Collections.ObjectModel;
using Railways.ViewModel.Messages;
using GalaSoft.MvvmLight.Messaging;
using Railways.View;
using Railways.Model.ModelBuilder;
using System.Windows.Controls;

namespace Railways.ViewModel
{
    public class VoyageEditViewModel : ViewModelBase
    {
        private String _voyageNum;
        private Voyage _voyage;
        public String VoyageNum
        {
            get { return _voyageNum; }
            set 
            {
                _voyageNum = value;
                RaisePropertyChanged("VoyageNum");
            }
        }
        public VoyageEditViewModel()
        {
            Messenger.Default.Register<SendTrainInfoMessage>(this, (msg) =>
            {
                SetVoyageInfo(msg.TrainId);
            });
        }

        private void SetVoyageInfo(int trainId) 
        {
            this._voyage = VoyageBuilder.GetVoyageOfTrain(trainId);
            this.VoyageNum = "№ рейса: " + _voyage.Id.ToString();
        }

    }
}

﻿using Intouch.Edm.Models;
using Intouch.Edm.Models.Dtos.CreateScenario;
using Intouch.Edm.Services;
using Intouch.Edm.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Intouch.Edm.ViewModels
{
    public class NewScenarioViewModel : BaseViewModel
    {

        ICommand _saveCommand;
        CreateEmergencyScenario.Picture picture = new CreateEmergencyScenario.Picture();
        public ICommand SaveClicked => _saveCommand
                ?? (_saveCommand = new Command(async () => await ExecuteSaveClicked()));

        async System.Threading.Tasks.Task ExecuteSaveClicked()
        {
            if (IsUploadingImage)
            {
                await Application.Current.MainPage.DisplayAlert("UYARI", "Resim yükleniyor. Lütfen bekleyiniz.", "TAMAM");
                return;
            }
            string selectedAction = await Application.Current.MainPage.DisplayActionSheet("Bildirimi göndermek istediğinize emin misiniz?", "Evet", "Hayır");
            switch (selectedAction)
            {
                case "Evet":
                    await CreateNotification();

                    break;
                case "Hayır":
                    break;
            }
        }

        public async Task Init()
        {

        }

        async System.Threading.Tasks.Task CreateNotification()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                if (!string.IsNullOrEmpty(Edm.Helpers.Settings.ContentId))
                {
                    picture = Edm.Helpers.Settings.GetPictureDetail();
                }
                else
                    picture = null;
                var newItem = new CreateEmergencyScenario.RootObject()
                {
                    userId = Convert.ToInt32(Helpers.Settings.UserId),
                    subjectType = SubjectId,
                    eventTypeId = EventId,
                    siteId = locationId,
                    sourceId = SourceId,
                    impactAreaId = ImpactAreaId,
                    picture = picture
                };

                var resultCreateScenario = await DataService.CreateEmergencyScenario(newItem);
                if (resultCreateScenario)
                {
                    await Application.Current.MainPage.DisplayAlert("BAŞARILI", "İşlem Tamamlanmıştır.", "TAMAM");
                    await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("BAŞARISIZ", "İşlem Sırasında Hata Alındı.", "TAMAM");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public List<ComboboxItem> ListSubject
        {
            get;
            set;
        }
        public List<ComboboxItem> ListEvent
        {
            get;
            set;
        }
        public NewScenarioViewModel()
        {
            Initialize();

        }
        public NewScenarioViewModel(int selectedId)
        {
            Initialize(selectedId);

        }

        public async void RetrieveImpactArea(int locationId)
        {
            var impactAreatList = await DataService.GetImpactAreaAsync(locationId);
            ImpactAreaCombobox = PickerService.GetImpactArea(impactAreatList);
        }

        private async void Initialize(int selectedEventId = 0)
        {
            int selectedSubjectId = selectedEventId != Convert.ToInt32(Events.BusinessContuniuty) ? Convert.ToInt32(Subjects.Emergency) : Convert.ToInt32(Subjects.BusinessContuniuty);
            var sourcetList = await DataService.GetSourcesAsync(selectedEventId.ToString(), selectedSubjectId);
            var eventList = await DataService.GetEventsAsync(selectedSubjectId);
            var locationList = await DataService.GetLocationAsync();
            SourceCombobox = PickerService.GetSource(sourcetList);
            SubjectCombobox = PickerService.GetSubject();
            EventCombobox = PickerService.GetEvent(eventList);
            LocationCombobox = PickerService.GetLocation(locationList);

            SelectedSubject = new ComboboxItem();
            SelectedEvent = new ComboboxItem();
            if (selectedEventId == Convert.ToInt32(Events.WaterFlood))
            {
                SelectedSubject = SubjectCombobox.Where(p => p.Id == 1).First();
                SelectedEvent = EventCombobox.Where(p => p.Id == selectedEventId).First();
            }
            else if (selectedEventId == Convert.ToInt32(Events.Fire))
            {
                SelectedSubject = SubjectCombobox.Where(p => p.Id == 1).First();
                SelectedEvent = EventCombobox.Where(p => p.Id == selectedEventId).First();
            }
            else if (selectedEventId == Convert.ToInt32(Events.Earthqueke))
            {
                SelectedSubject = SubjectCombobox.Where(p => p.Id == 1).First();
                SelectedEvent = EventCombobox.Where(p => p.Id == selectedEventId).First();
            }
            else if (selectedEventId == Convert.ToInt32(Events.BusinessContuniuty))
            {
                SelectedSubject = SubjectCombobox.Where(p => p.Id == 2).First();
            }
        }


        private ComboboxItem _selectedEvent;
        public ComboboxItem SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                SetProperty(ref _selectedEvent, value);
                EventId = SelectedEvent.Id;
            }
        }

        private ComboboxItem _selectedSource;
        public ComboboxItem SelectedSource
        {
            get
            {
                return _selectedSource;
            }
            set
            {
                SetProperty(ref _selectedSource, value);
                SourceId = SelectedSource.Id;
            }
        }

        private ComboboxItem _selectedImpactArea;
        public ComboboxItem SelectedImpactArea
        {
            get
            {
                return _selectedImpactArea;
            }
            set
            {
                SetProperty(ref _selectedImpactArea, value);
                ImpactAreaId = SelectedImpactArea.Id;
            }
        }

        private ComboboxItem _selectedLocation;
        public ComboboxItem SelectedLocation
        {
            get
            {
                return _selectedLocation;
            }
            set
            {
                SetProperty(ref _selectedLocation, value);
                locationId = SelectedLocation.Id;
                RetrieveImpactArea(locationId);
            }
        }

        private ComboboxItem _selectedSubject;
        public ComboboxItem SelectedSubject
        {
            get
            {
                return _selectedSubject;
            }
            set
            {

                SetProperty(ref _selectedSubject, value);
                SubjectText = "Subject : " + _selectedSubject.Name;
                SubjectId = _selectedSubject.Id;
            }
        }

        private int _subjectId;
        public int SubjectId
        {
            get
            {
                return _subjectId;
            }
            set
            {
                SetProperty(ref _subjectId, value);
            }
        }

        private int _eventId;
        public int EventId
        {
            get
            {
                return _eventId;
            }
            set
            {
                SetProperty(ref _eventId, value);
            }
        }

        private int _siteId;
        public int SiteId
        {
            get
            {
                return _siteId;
            }
            set
            {
                SetProperty(ref _siteId, value);
            }
        }

        private int _sourceId;
        public int SourceId
        {
            get
            {
                return _sourceId;
            }
            set
            {
                SetProperty(ref _sourceId, value);
            }
        }

        private int _locationId;
        public int locationId
        {
            get
            {
                return _locationId;
            }
            set
            {
                SetProperty(ref _locationId, value);
            }
        }

        private int _impactAreaId;
        public int ImpactAreaId
        {
            get
            {
                return _impactAreaId;
            }
            set
            {
                SetProperty(ref _impactAreaId, value);
            }
        }

        private string _subjectText;
        public string SubjectText
        {
            get
            {
                return _subjectText;
            }
            set
            {
                SetProperty(ref _subjectText, value);
            }
        }
        double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                SetProperty(ref _latitude, value);
            }
        }

        double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                SetProperty(ref _longitude, value);
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                SetProperty(ref _imageSource, value);
            }
        }
        private bool _isUploadingImage;
        public bool IsUploadingImage
        {
            get
            {
                return _isUploadingImage;
            }
            set
            {
                SetProperty(ref _isUploadingImage, value);
            }
        }


        ICommand _sheetClicked;
        public ICommand SheetSimpleClicked => _sheetClicked
                ?? (_sheetClicked = new Command(async () => await SheetSimpleCommand()));

        async System.Threading.Tasks.Task SheetSimpleCommand()
        {
            string selectedAction = await Application.Current.MainPage.DisplayActionSheet("Fotoğraf Yükle", "Fotoğraf Çek", "Galeriden Yükle");
            switch (selectedAction)
            {
                case "Fotoğraf Çek":
                    TakePhoto();
                    break;
                case "Galeriden Yükle":
                    SelectPhotoFromGallery();
                    break;
            }
        }

        public async void TakePhoto()
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("UYARI", "Cihazınızın kamerası aktif değil!", "OK");
                return;
            }
            IsUploadingImage = true;

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                       new Plugin.Media.Abstractions.StoreCameraMediaOptions
                       {
                           Directory = "temp",
                           Name = DateTime.Now + ".jpg",
                           DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                           CompressionQuality = 50
                       });
                if (file == null)
                    return;
                var imageFile = file;
                var resultPicture = await DataService.UploadImageAsync(imageFile.GetStream(), $"Test_Photo - {DateTime.Now}" + ".jpg");

                Edm.Helpers.Settings.SetUploadResult(resultPicture);
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("HATA", "Fotoğraf çekilirken hata oluştu", "OK");
            }
            IsUploadingImage = false;
        }

        private async void SelectPhotoFromGallery()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("UYARI", "Galeriye erişme yetkiniz yok!", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new
                                  Plugin.Media.Abstractions.PickMediaOptions
            {
                CompressionQuality = 50
            });
            IsUploadingImage = true;
            try
            {
                if (file == null)
                    return;

                var imageFile = file;
                var resultPicture = await DataService.UploadImageAsync(imageFile.GetStream(), $"Test_Photo - {DateTime.Now}" + ".jpg");
                Edm.Helpers.Settings.SetUploadResult(resultPicture);

                ImageSource = ImageSource.FromStream(() =>
                  {
                      var stream = file.GetStream();
                      file.Dispose();
                      return stream;
                  });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("HATA", "Fotoğraf gönderilirken hata oluştu", "OK");
            }
            IsUploadingImage = false;
        }

        ObservableCollection<ComboboxItem> _SubjectCombobox;
        public ObservableCollection<ComboboxItem> SubjectCombobox
        {
            get { return _SubjectCombobox; }
            set
            {
                _SubjectCombobox = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<ComboboxItem> _EventCombobox;
        public ObservableCollection<ComboboxItem> EventCombobox
        {
            get { return _EventCombobox; }
            set
            {
                _EventCombobox = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<ComboboxItem> _SourceCombobox;
        public ObservableCollection<ComboboxItem> SourceCombobox
        {
            get { return _SourceCombobox; }
            set
            {
                _SourceCombobox = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<ComboboxItem> _ImpactAreaCombobox;
        public ObservableCollection<ComboboxItem> ImpactAreaCombobox
        {
            get { return _ImpactAreaCombobox; }
            set
            {
                _ImpactAreaCombobox = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<ComboboxItem> _LocationCombobox;
        public ObservableCollection<ComboboxItem> LocationCombobox
        {
            get { return _LocationCombobox; }
            set
            {
                _LocationCombobox = value;
                OnPropertyChanged();
            }
        }
    }
}
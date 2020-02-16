﻿using Intouch.Edm.Models.Dtos.LookupDto;
using EventType = Intouch.Edm.Models.Dtos.LookupDto.EventTypeLookup;
using ImpactArea = Intouch.Edm.Models.Dtos.LookupDto.ImpactAreaLookup;
using Location = Intouch.Edm.Models.Dtos.LookupDto.LocationLookup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Intouch.Edm.Services
{
    public class PickerService
    {
        public static ObservableCollection<ComboboxItem> GetSubject()
        {
            var subjects = new ObservableCollection<ComboboxItem>()
            {
                new ComboboxItem() { Id= 1 , Name="Acil Durum"},
                new ComboboxItem() { Id= 2 , Name="İş Sürekliliği"}
            };
            return subjects;
        }
        public static ObservableCollection<ComboboxItem> GetEvent(EventType.RootObject events)
        {
            var eventList = new ObservableCollection<ComboboxItem>();
            foreach (var source in events.result.items)
            {
                ComboboxItem sourceComboboxItem = new ComboboxItem();
                sourceComboboxItem.Name = source.eventType.name;
                sourceComboboxItem.Id = source.eventType.id;
                eventList.Add(sourceComboboxItem);
            }
            return eventList;
        }

        internal static ObservableCollection<ComboboxItem> GetSource(Models.Dtos.SourceLookupDto.RootObject sources)
        {
            var sourceList = new ObservableCollection<ComboboxItem>();
            foreach (var source in sources.result.items)
            {
                ComboboxItem sourceComboboxItem = new ComboboxItem();
                sourceComboboxItem.Name = source.source.name;
                sourceComboboxItem.Id = source.source.id;
                sourceList.Add(sourceComboboxItem);
            }

            return sourceList;
        }

        internal static ObservableCollection<ComboboxItem> GetImpactArea(ImpactArea.RootObject impactAreatList)
        {
            var impactAreaList = new ObservableCollection<ComboboxItem>();
            foreach (var impactArea in impactAreatList.result.items)
            {
                ComboboxItem impactAreaComboboxItem = new ComboboxItem();
                impactAreaComboboxItem.Name = impactArea.impactArea.name;
                impactAreaComboboxItem.Id = impactArea.impactArea.id;
                impactAreaList.Add(impactAreaComboboxItem);
            }

            return impactAreaList;
        }

        internal static ObservableCollection<ComboboxItem> GetLocation(Location.RootObject locationList)
        {
            var locations = new ObservableCollection<ComboboxItem>();
            foreach (var location in locationList.result.items)
            {
                ComboboxItem locationComboboxItem = new ComboboxItem();
                locationComboboxItem.Name = location.site.name;
                locationComboboxItem.Id = location.site.id;
                locations.Add(locationComboboxItem);
            }

            return locations;
        }
    }

    public enum Subjects
    {
        Emergency = 1,
        BusinessContuniuty = 5
    }

    public enum Events
    {
        WaterFlood = 1,
        Earthqueke = 2,
        Fire = 3,
        BusinessContuniuty = 5
    }
}

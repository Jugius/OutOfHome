﻿using OutOfHome.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OutOfHome.Helpers.Cities
{
    public static class Defaults
    {
        private static List<City> _cities;
        private static Dictionary<int, City> _dicDoors;
        private static Dictionary<int, City> _dicBma;
        private static Dictionary<int, City> _dicNames;
        public static List<City> Cities 
        {
            get {
                if(_cities == null || _cities.Count == 0)
                    _cities = InitializeCities();
                return _cities;
            }
        }
        private static Dictionary<int, City> DicDoors => _dicDoors ??= Cities.Where(a => a.DoorsId.HasValue).ToDictionary(a => a.DoorsId.GetValueOrDefault(), a => a);
        private static Dictionary<int, City> DicBma => _dicBma ??= Cities.Where(a => a.BmaId.HasValue).ToDictionary(a => a.BmaId.GetValueOrDefault(), a => a);
        private static Dictionary<int, City> DicNames => _dicNames ??= Cities.ToDictionary(a => a.Name.GetHashCode(), a => a);
        public static City GetCityByDoorsId(int id) => DicDoors.TryGetValue(id, out City city) ? city : null;
        public static City GetCityByBmaId(int id) => DicBma.TryGetValue(id, out City city) ? city : null;
        public static City GetCityByName(string name) => DicNames.TryGetValue(name.GetHashCode(), out City city) ? city : null;
        private static List<City> InitializeCities()
        {
            return new List<City>(200)
            {
                new City { Name = "Александрия", Region = RegionsDb.Kirovogradskaya, BmaId = 64, DoorsId = 49, DoorsShortName = "Ole", Center = new Models.Location(48.66422, 33.09204), IsCapital = false, IsRegionalCenter= false, Population = 79289},
                new City { Name = "Ахтырка", Region = RegionsDb.Sumskaya, BmaId = 122, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 47829},
                new City { Name = "Бахмут", Region = RegionsDb.Donetskaya, BmaId = 128, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 74072},
                new City { Name = "Белая Церковь", Region = RegionsDb.Kievskaya, BmaId = 39, DoorsId = 31, DoorsShortName = "Bil", Center = new Models.Location(49.80385, 30.10897), IsCapital = false, IsRegionalCenter= false, Population = 208944},
                new City { Name = "Белополье", Region = RegionsDb.Sumskaya, BmaId = 119, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 16152},
                new City { Name = "Бердянск", Region = RegionsDb.Zaporozhskaya, BmaId = 59, DoorsId = 41, DoorsShortName = "Ber", Center = new Models.Location(46.75981, 36.79269), IsCapital = false, IsRegionalCenter= false, Population = 110455},
                new City { Name = "Борислав", Region = RegionsDb.Lvovskaya, BmaId = 95, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 33537},
                new City { Name = "Борисполь", Region = RegionsDb.Kievskaya, BmaId = 56, DoorsId = 29, DoorsShortName = "Bor", Center = new Models.Location(50.3655, 30.81287), IsCapital = false, IsRegionalCenter= false, Population = 62281},
                new City { Name = "Броды", Region = RegionsDb.Lvovskaya, BmaId = 96, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 23517},
                new City { Name = "Вараш", Region = RegionsDb.Rovenskaya, BmaId = 97, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 42350},
                new City { Name = "Винница", Region = RegionsDb.Vinnitskaya, BmaId = 26, DoorsId = 12, DoorsShortName = "Vin", Center = new Models.Location(49.2284, 28.5009), IsCapital = false, IsRegionalCenter= true, Population = 369839},
                new City { Name = "Днепр", Region = RegionsDb.Dnepropetrovskaya, BmaId = 10, DoorsId = 4, DoorsShortName = "Dne", Center = new Models.Location(48.4569, 34.9935), IsCapital = false, IsRegionalCenter= true, Population = 998103},
                new City { Name = "Дрогобыч", Region = RegionsDb.Lvovskaya, BmaId = 66, DoorsId = 55, DoorsShortName = "Dro", Center = new Models.Location(49.34923, 23.51748), IsCapital = false, IsRegionalCenter= false, Population = 76044},
                new City { Name = "Дубно", Region = RegionsDb.Rovenskaya, BmaId = 98, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 37563},
                new City { Name = "Житомир", Region = RegionsDb.Zhitomirskaya, BmaId = 29, DoorsId = 13, DoorsShortName = "Zhy", Center = new Models.Location(50.2756, 28.689), IsCapital = false, IsRegionalCenter= true, Population = 265240},
                new City { Name = "Запорожье", Region = RegionsDb.Zaporozhskaya, BmaId = 6, DoorsId = 5, DoorsShortName = "Zap", Center = new Models.Location(47.85217, 35.14333), IsCapital = false, IsRegionalCenter= true, Population = 738728},
                new City { Name = "Здолбунов", Region = RegionsDb.Rovenskaya, BmaId = 99, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 24766},
                new City { Name = "Ивано-Франковск", Region = RegionsDb.IvanoFrankovskaya, BmaId = 12, DoorsId = 25, DoorsShortName = "Ifr", Center = new Models.Location(48.91892, 24.71519), IsCapital = false, IsRegionalCenter= true, Population = 236602},
                new City { Name = "Каменское", Region = RegionsDb.Dnepropetrovskaya, BmaId = 51, DoorsId = 20, DoorsShortName = "Dnp", Center = new Models.Location(48.52399, 34.61563), IsCapital = false, IsRegionalCenter= true, Population = 233358},
                new City { Name = "Киев", Region = RegionsDb.Kievskaya, BmaId = 1, DoorsId = 1, DoorsShortName = "Kie", Center = new Models.Location(50.45, 30.52333), IsCapital = true, IsRegionalCenter= true, Population = 2950819},
                new City { Name = "Ковель", Region = RegionsDb.Volynskaya, BmaId = 65, DoorsId = 54, DoorsShortName = "Kov", Center = new Models.Location(51.22245, 24.70655), IsCapital = false, IsRegionalCenter= false, Population = 68603},
                new City { Name = "Коломыя", Region = RegionsDb.IvanoFrankovskaya, BmaId = 100, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 61269},
                new City { Name = "Коростень", Region = RegionsDb.Zhitomirskaya, BmaId = 101, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 63300},
                new City { Name = "Коростышев", Region = RegionsDb.Zhitomirskaya, BmaId = 124, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 25084},
                new City { Name = "Краматорск", Region = RegionsDb.Donetskaya, BmaId = 41, DoorsId = 40, DoorsShortName = "Kra", Center = new Models.Location(48.73648, 37.57931), IsCapital = false, IsRegionalCenter= false, Population = 153911},
                new City { Name = "Кременец", Region = RegionsDb.Ternopolskaya, BmaId = 102, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 21063},
                new City { Name = "Кривой Рог", Region = RegionsDb.Dnepropetrovskaya, BmaId = 11, DoorsId = 27, DoorsShortName = "Kry", Center = new Models.Location(48.02034, 33.45372), IsCapital = false, IsRegionalCenter= false, Population = 624579},
                new City { Name = "Кропивницкий", Region = RegionsDb.Kirovogradskaya, BmaId = 30, DoorsId = 16, DoorsShortName = "Kir", Center = new Models.Location(48.5144, 32.2518), IsCapital = false, IsRegionalCenter= true, Population = 227413},
                new City { Name = "Лебедин", Region = RegionsDb.Sumskaya, BmaId = 120, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 24853},
                new City { Name = "Лисичанск", Region = RegionsDb.Luganskaya, BmaId = 129, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 97251},
                new City { Name = "Луцк", Region = RegionsDb.Volynskaya, BmaId = 13, DoorsId = 18, DoorsShortName = "Lut", Center = new Models.Location(50.749, 25.34), IsCapital = false, IsRegionalCenter= true, Population = 216887},
                new City { Name = "Львов", Region = RegionsDb.Lvovskaya, BmaId = 4, DoorsId = 7, DoorsShortName = "Lvo", Center = new Models.Location(49.83, 24.02), IsCapital = false, IsRegionalCenter= true, Population = 724713},
                new City { Name = "Малин", Region = RegionsDb.Zhitomirskaya, BmaId = 104, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 26056},
                new City { Name = "Мариуполь", Region = RegionsDb.Donetskaya, BmaId = 32, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 440367},
                new City { Name = "Могилев-Подольский", Region = RegionsDb.Vinnitskaya, BmaId = 105, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 30589},
                new City { Name = "Мукачево", Region = RegionsDb.Zakarpatskaya, BmaId = 61, DoorsId = 43, DoorsShortName = "Muk", Center = new Models.Location(48.44059, 22.71757), IsCapital = false, IsRegionalCenter= false, Population = 85881},
                new City { Name = "Николаев", Region = RegionsDb.Nikolaevskaya, BmaId = 22, DoorsId = 15, DoorsShortName = "Nkl", Center = new Models.Location(46.9548, 32.0201), IsCapital = false, IsRegionalCenter= true, Population = 483186},
                new City { Name = "Никополь", Region = RegionsDb.Dnepropetrovskaya, BmaId = 106, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 110669},
                new City { Name = "Новая Каховка", Region = RegionsDb.Khersonskaya, BmaId = 107, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 45819},
                new City { Name = "Нововолынск", Region = RegionsDb.Volynskaya, BmaId = 108, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 51564},
                new City { Name = "Одесса", Region = RegionsDb.Odesskaya, BmaId = 5, DoorsId = 2, DoorsShortName = "Ode", Center = new Models.Location(46.4689, 30.7384), IsCapital = false, IsRegionalCenter= true, Population = 1013159},
                new City { Name = "Павлоград", Region = RegionsDb.Dnepropetrovskaya, BmaId = 50, DoorsId = 39, DoorsShortName = "Pav", Center = new Models.Location(48.52996, 35.88761), IsCapital = false, IsRegionalCenter= false, Population = 105238},
                new City { Name = "Пирятин", Region = RegionsDb.Poltavskaya, BmaId = 110, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 15391},
                new City { Name = "Покровск", Region = RegionsDb.Donetskaya, BmaId = 130, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 62449},
                new City { Name = "Полтава", Region = RegionsDb.Poltavskaya, BmaId = 23, DoorsId = 9, DoorsShortName = "Pol", Center = new Models.Location(49.5957, 34.5539), IsCapital = false, IsRegionalCenter= true, Population = 288324},
                new City { Name = "Путивль", Region = RegionsDb.Sumskaya, BmaId = 111, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 15433},
                new City { Name = "Рава-Русская", Region = RegionsDb.Lvovskaya, BmaId = 112, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8701},
                new City { Name = "Ровно", Region = RegionsDb.Rovenskaya, BmaId = 24, DoorsId = 24, DoorsShortName = "Rov", Center = new Models.Location(50.61738, 26.243), IsCapital = false, IsRegionalCenter= true, Population = 246535},
                new City { Name = "Ромны", Region = RegionsDb.Sumskaya, BmaId = 71, DoorsId = 61, DoorsShortName = "Rom", Center = new Models.Location(50.73989, 33.47309), IsCapital = false, IsRegionalCenter= false, Population = 39532},
                new City { Name = "Сарны", Region = RegionsDb.Rovenskaya, BmaId = 113, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 29205},
                new City { Name = "Северодонецк", Region = RegionsDb.Luganskaya, BmaId = 131, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 103479},
                new City { Name = "Смела", Region = RegionsDb.Cherkasskaya, BmaId = 114, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 67530},
                new City { Name = "Сумы", Region = RegionsDb.Sumskaya, BmaId = 14, DoorsId = 23, DoorsShortName = "Sum", Center = new Models.Location(50.9019, 34.8146), IsCapital = false, IsRegionalCenter= true, Population = 263448},
                new City { Name = "Тернополь", Region = RegionsDb.Ternopolskaya, BmaId = 25, DoorsId = 26, DoorsShortName = "Ter", Center = new Models.Location(49.5666, 25.6), IsCapital = false, IsRegionalCenter= true, Population = 221820},
                new City { Name = "Трассы Киевская обл", Region = RegionsDb.Kievskaya, BmaId = 54, DoorsId = 56, DoorsShortName = "Kio", Center = new Models.Location(50.35549, 30.71627), IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Львовская обл", Region = RegionsDb.Lvovskaya, BmaId = 103, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Одесская обл", Region = RegionsDb.Odesskaya, BmaId = 109, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Тростянец", Region = RegionsDb.Sumskaya, BmaId = 89, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 20238},
                new City { Name = "Трускавец", Region = RegionsDb.Lvovskaya, BmaId = 67, DoorsId = 57, DoorsShortName = "Tru", Center = new Models.Location(49.27774, 23.51166), IsCapital = false, IsRegionalCenter= false, Population = 28705},
                new City { Name = "Ужгород", Region = RegionsDb.Zakarpatskaya, BmaId = 31, DoorsId = 21, DoorsShortName = "Uzh", Center = new Models.Location(48.61725, 22.29329), IsCapital = false, IsRegionalCenter= true, Population = 114897},
                new City { Name = "Харьков", Region = RegionsDb.Kharkovskaya, BmaId = 3, DoorsId = 3, DoorsShortName = "Har", Center = new Models.Location(49.9745, 36.2764), IsCapital = false, IsRegionalCenter= true, Population = 1446107},
                new City { Name = "Херсон", Region = RegionsDb.Khersonskaya, BmaId = 19, DoorsId = 10, DoorsShortName = "Khr", Center = new Models.Location(46.6479, 32.6175), IsCapital = false, IsRegionalCenter= true, Population = 289096},
                new City { Name = "Хмельницкий", Region = RegionsDb.Khmelnitskaya, BmaId = 20, DoorsId = 17, DoorsShortName = "Khm", Center = new Models.Location(49.4197, 26.9933), IsCapital = false, IsRegionalCenter= true, Population = 271263},
                new City { Name = "Червоноград", Region = RegionsDb.Lvovskaya, BmaId = 116, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 66504},
                new City { Name = "Черкассы", Region = RegionsDb.Cherkasskaya, BmaId = 38, DoorsId = 8, DoorsShortName = "Che", Center = new Models.Location(49.43199, 32.056), IsCapital = false, IsRegionalCenter= true, Population = 276360},
                new City { Name = "Чернигов", Region = RegionsDb.Chernigovskaya, BmaId = 36, DoorsId = 22, DoorsShortName = "Chr", Center = new Models.Location(51.50272, 31.28437), IsCapital = false, IsRegionalCenter= true, Population = 288268},
                new City { Name = "Черновцы", Region = RegionsDb.Chernovitskaya, BmaId = 15, DoorsId = 19, DoorsShortName = "Chc", Center = new Models.Location(48.29972, 25.93421), IsCapital = false, IsRegionalCenter= true, Population = 266533},
                new City { Name = "Чоп", Region = RegionsDb.Zakarpatskaya, BmaId = 60, DoorsId = 42, DoorsShortName = "Cho", Center = new Models.Location(48.42978, 22.20789), IsCapital = false, IsRegionalCenter= false, Population = 8897},
                new City { Name = "Чортков", Region = RegionsDb.Ternopolskaya, BmaId = 126, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 28858},
                new City { Name = "Шостка", Region = RegionsDb.Sumskaya, BmaId = 123, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 75024},
                new City { Name = "Кременчуг", Region = RegionsDb.Poltavskaya, BmaId = null, DoorsId = 32, DoorsShortName = "Kre", Center = new Models.Location(49.09277, 33.44212), IsCapital = false, IsRegionalCenter= false, Population = 220065},
                new City { Name = "Мелитополь", Region = RegionsDb.Zaporozhskaya, BmaId = null, DoorsId = 36, DoorsShortName = "Mel", Center = new Models.Location(46.84371, 35.35699), IsCapital = false, IsRegionalCenter= false, Population = 153112},
                new City { Name = "Каменец-Подольский", Region = RegionsDb.Khmelnitskaya, BmaId = null, DoorsId = 37, DoorsShortName = "Kam", Center = new Models.Location(48.69043, 26.57964), IsCapital = false, IsRegionalCenter= false, Population = 99755},
                new City { Name = "Бердичев", Region = RegionsDb.Zhitomirskaya, BmaId = null, DoorsId = 38, DoorsShortName = "Bev", Center = new Models.Location(49.89284, 28.60814), IsCapital = false, IsRegionalCenter= false, Population = 75439},
                new City { Name = "Калуш", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = 45, DoorsShortName = "Kal", Center = new Models.Location(49.04394, 24.35615), IsCapital = false, IsRegionalCenter= false, Population = 66406},
                new City { Name = "Шепетовка", Region = RegionsDb.Khmelnitskaya, BmaId = null, DoorsId = 47, DoorsShortName = "She", Center = new Models.Location(50.17839, 27.06039), IsCapital = false, IsRegionalCenter= false, Population = 41599},
                new City { Name = "Лубны", Region = RegionsDb.Poltavskaya, BmaId = null, DoorsId = 50, DoorsShortName = "Lub", Center = new Models.Location(50.01831, 32.98833), IsCapital = false, IsRegionalCenter= false, Population = 45379},
                new City { Name = "Светловодск", Region = RegionsDb.Kirovogradskaya, BmaId = null, DoorsId = 58, DoorsShortName = "Sve", Center = new Models.Location(49.0515, 33.20309), IsCapital = false, IsRegionalCenter= false, Population = 44857},
                new City { Name = "Прилуки", Region = RegionsDb.Chernigovskaya, BmaId = null, DoorsId = 63, DoorsShortName = "Pry", Center = new Models.Location(50.58877, 32.382), IsCapital = false, IsRegionalCenter= false, Population = 54167},
                new City { Name = "Берегово", Region = RegionsDb.Zakarpatskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 23732},
                new City { Name = "Бережаны", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 17697},
                new City { Name = "Богородчаны", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8222},
                new City { Name = "Болехов", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 10476},
                new City { Name = "Борщов", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 10862},
                new City { Name = "Бровары", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 106346},
                new City { Name = "Брюховичи", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 6223},
                new City { Name = "Буковель", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Бурштын", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 15039},
                new City { Name = "Буча", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 35162},
                new City { Name = "Бучач", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 12414},
                new City { Name = "Васильевка", Region = RegionsDb.Zaporozhskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 13166},
                new City { Name = "Васильков", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 37696},
                new City { Name = "Верховина", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 5829},
                new City { Name = "Винники", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 17642},
                new City { Name = "Вишневое", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 40919},
                new City { Name = "Владимир-Волынский", Region = RegionsDb.Volynskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 38577},
                new City { Name = "Галич", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 6196},
                new City { Name = "Горишние Плавни", Region = RegionsDb.Poltavskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 51215},
                new City { Name = "Городенка", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 9113},
                new City { Name = "Городище", Region = RegionsDb.Cherkasskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 13609},
                new City { Name = "Гусятин", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 7083},
                new City { Name = "Гута", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 738},
                new City { Name = "Дергачи", Region = RegionsDb.Kharkovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 17781},
                new City { Name = "Долина", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 20691},
                new City { Name = "Зборов", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 6695},
                new City { Name = "Киверцы", Region = RegionsDb.Volynskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 14050},
                new City { Name = "Кирилловка", Region = RegionsDb.Zaporozhskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 3464},
                new City { Name = "Козова", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8989},
                new City { Name = "Косов", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8522},
                new City { Name = "Крюковщина", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Лановцы", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8434},
                new City { Name = "Любомль", Region = RegionsDb.Volynskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 10457},
                new City { Name = "Малехов", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Марганец", Region = RegionsDb.Dnepropetrovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 46616},
                new City { Name = "Мерефа", Region = RegionsDb.Kharkovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 21794},
                new City { Name = "Миргород", Region = RegionsDb.Poltavskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 39575},
                new City { Name = "Монастыриска", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 5618},
                new City { Name = "Моршин", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 5793},
                new City { Name = "Надворная", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 22447},
                new City { Name = "Новомосковск", Region = RegionsDb.Dnepropetrovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 70550},
                new City { Name = "Новоселовка", Region = RegionsDb.Dnepropetrovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Песочин", Region = RegionsDb.Kharkovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 23414},
                new City { Name = "Подволочиск", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 7843},
                new City { Name = "Почаев", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 7735},
                new City { Name = "Приморск", Region = RegionsDb.Zaporozhskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 11679},
                new City { Name = "Рогатин", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 7797},
                new City { Name = "Самбор", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 34823},
                new City { Name = "Святогорск", Region = RegionsDb.Donetskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 4395},
                new City { Name = "Синельниково", Region = RegionsDb.Dnepropetrovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 30556},
                new City { Name = "Скалат", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 3888},
                new City { Name = "Славское", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 3551},
                new City { Name = "Снятын", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 9942},
                new City { Name = "Солонка", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Софиевская Борщаговка", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Стрый", Region = RegionsDb.Lvovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 59488},
                new City { Name = "Теребовля", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 13425},
                new City { Name = "Тлумач", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8886},
                new City { Name = "Токмак", Region = RegionsDb.Zaporozhskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 31016},
                new City { Name = "Трассы Волынская обл", Region = RegionsDb.Volynskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Днепропетровская обл", Region = RegionsDb.Dnepropetrovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Запорожская обл", Region = RegionsDb.Zaporozhskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Ивано-Франковская обл", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Тернопольская обл", Region = RegionsDb.Ternopolskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Трассы Харьковская обл", Region = RegionsDb.Kharkovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Тысменица", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 9287},
                new City { Name = "Чайки", Region = RegionsDb.Kievskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Ягодин", Region = RegionsDb.Volynskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 0},
                new City { Name = "Яремче", Region = RegionsDb.IvanoFrankovskaya, BmaId = null, DoorsId = null, DoorsShortName = null, Center = null, IsCapital = false, IsRegionalCenter= false, Population = 8094},
            };
        }
    }
}
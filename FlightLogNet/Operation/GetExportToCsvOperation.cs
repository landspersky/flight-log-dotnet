namespace FlightLogNet.Operation
{
    using System;
    using System.Text;
    using FlightLogNet.Models;
    using Repositories.Interfaces;

    public class GetExportToCsvOperation(IFlightRepository flightRepository)
    {
        private readonly string[] _headers = [
            "Datum",
            "Typ",
            "Imatrikulace",
            "Osádka",
            "Úkol",
            "Start",
            "Přistání",
            "Doba letu",
            ];

        private static string ToCSVLine(params object[] fields) => String.Join(";", fields);

        private static string FormatDuration(TimeSpan duration) => $"{(int)duration.TotalHours}°{duration.Minutes:00}'";

        public byte[] Execute()
        {
            // Done 5.1: Naimplementujte export do CSV
            // TIP: CSV soubor je pouze string, který se dá vytvořit pomocí třídy StringBuilder
            // TIP: Do bytové reprezentace je možné jej převést například pomocí metody: Encoding.UTF8.GetBytes(..)
            var report = flightRepository.GetReport();
            var sb = new StringBuilder();
            sb.AppendLine(ToCSVLine(_headers));

            foreach (var reportModel in report)
            {
                if (reportModel.Towplane is FlightModel towplane)
                {
                    sb.AppendLine(ToCSVLine([
                        towplane.TakeoffTime.Date.ToString("dd/MM/yyyy"),
                        towplane.Airplane.Type,
                        towplane.Airplane.Immatriculation,
                        towplane.Pilot.LastName,
                        "Tahač",
                        towplane.TakeoffTime.ToString("HH:mm:ss"),
                        towplane.LandingTime?.ToString("HH:mm:ss"),
                        towplane.LandingTime is null ? "" : FormatDuration(towplane.LandingTime.Value - towplane.TakeoffTime)
                        ]));
                }

                if (reportModel.Glider is FlightModel glider)
                {
                    sb.AppendLine(ToCSVLine([
                        glider.TakeoffTime.Date.ToString("dd/MM/yyyy"),
                        glider.Airplane.Type,
                        glider.Airplane.Immatriculation,
                        glider.Pilot.LastName,
                        "VLEK",
                        glider.TakeoffTime.ToString("HH:mm:ss"),
                        glider.LandingTime?.ToString("HH:mm:ss"),
                        glider.LandingTime is null ? "" : FormatDuration(glider.LandingTime.Value - glider.TakeoffTime)
                        ]));
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}

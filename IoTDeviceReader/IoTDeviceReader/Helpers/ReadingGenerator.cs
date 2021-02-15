using Bogus;
using IoTDeviceReader.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTDeviceReader.Helpers
{
    public static class ReadingGenerator
    {
        public static List<DeviceReading> GenerateReadings(int numberOfReadings)
        {
            var generatedReadings = new Faker<DeviceReading>()
                .RuleFor(i => i.DeviceReadingId, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.DeviceTempreature, (fake) => fake.Random.Double(0, 150.0))
                .RuleFor(i => i.Latitude, (fake) => fake.Random.Double(-180.0, 180.0))
                .RuleFor(i => i.Longitude, (fake) => fake.Random.Double(-180.0, 180.0))
                .RuleFor(i => i.DamageLevel, (fake) => fake.PickRandom(new List<string> { "Low", "Medium", "High"}))
                .RuleFor(i => i.DeviceLocation, (fake) => fake.PickRandom(new List<string> { "New Zealand", "USA", "UK", "Brazil", "Netherlands", "Italy", "Russia"}))
                .Generate(numberOfReadings);

            return generatedReadings;
        }
    }
}

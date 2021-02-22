using Bogus;
using IoTDeviceReader.Models;
using System;
using System.Collections.Generic;

namespace IoTDeviceReader.Helpers
{
    public static class ReadingGenerator
    {
        public static List<DeviceReading> GenerateReadings(int numberOfReadings)
        {
            var generatedManufacturer = new Faker<DeviceManufacturer>()
                .RuleFor(i => i.Name, (fake) => fake.PickRandom(new List<string> { "MyDevice", "Commodore", "ARM", "Raspberry Pi" }))
                .Generate();

            var generatedReadings = new Faker<DeviceReading>()
                .RuleFor(i => i.DeviceReadingId, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.DeviceTempreature, (fake) => fake.Random.Double(0, 150.0))
                .RuleFor(i => i.Latitude, (fake) => fake.Random.Double(-180.0, 180.0))
                .RuleFor(i => i.Longitude, (fake) => fake.Random.Double(-180.0, 180.0))
                .RuleFor(i => i.DamageLevel, (fake) => fake.PickRandom(new List<string> { "Low", "Medium", "High"}))
                .RuleFor(i => i.DeviceLocation, (fake) => fake.PickRandom(new List<string> { "New Zealand", "USA", "UK", "Brazil", "Netherlands", "Italy", "Russia"}))
                .RuleFor(i => i.Manufacturer, (fake) => generatedManufacturer)
                .Generate(numberOfReadings);

            return generatedReadings;
        }
    }
}

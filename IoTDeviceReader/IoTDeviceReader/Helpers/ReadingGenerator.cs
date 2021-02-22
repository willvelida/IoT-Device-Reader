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
            var generatedManufacturer = new Faker<Device>()
                .RuleFor(i => i.DeviceId, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.Name, (fake) => fake.PickRandom(new List<string> { "Alpha", "Beta", "Gamma", "Omega" }))
                .RuleFor(i => i.Manufacturer, (fake) => fake.PickRandom(new List<string> { "Alpha", "Beta", "Gamma", "Omega" }))
                .RuleFor(i => i.AgeInDays, (fake) => fake.Random.Int(1, 100))
                .RuleFor(i => i.DamageLevel, (fake) => fake.PickRandom(new List<string> { "Low", "Medium", "High" }))
                .Generate();

            var generatedReadings = new Faker<DeviceReading>()
                .RuleFor(i => i.DeviceReadingId, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.DeviceTempreature, (fake) => fake.Random.Double(0, 150.0))
                .RuleFor(i => i.Latitude, (fake) => fake.Random.Double(-180.0, 180.0))
                .RuleFor(i => i.Longitude, (fake) => fake.Random.Double(-180.0, 180.0))
                .RuleFor(i => i.DeviceLocation, (fake) => fake.PickRandom(new List<string> { "New Zealand", "USA", "UK", "Brazil", "Netherlands", "Italy", "Russia" }))
                .RuleFor(i => i.Device, (fake) => generatedManufacturer)
                .Generate(numberOfReadings);

            return generatedReadings;
        }

        public static List<Device> GenerateDevices(int numberOfDevices)
        {
            var generatedManufacturer = new Faker<Device>()
                .RuleFor(i => i.DeviceId, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.Name, (fake) => fake.PickRandom(new List<string> { "Alpha", "Beta", "Gamma", "Omega" }))
                .RuleFor(i => i.Manufacturer, (fake) => fake.PickRandom(new List<string> { "Alpha", "Beta", "Gamma", "Omega" }))
                .RuleFor(i => i.AgeInDays, (fake) => fake.Random.Int(1, 100))
                .RuleFor(i => i.DamageLevel, (fake) => fake.PickRandom(new List<string> { "Low", "Medium", "High" }))
                .Generate(numberOfDevices);

            return generatedManufacturer;
        }
    }
}

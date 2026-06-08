using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;

namespace Nitrogen.Services.Modbus.Configuration.Loading;

internal sealed class ModbusConfigLoader
{
    private const string ConfigurationFolderName = "Configuration";
    private const string ModbusFolderName = "Modbus";

    private const string ConnectionFileName = "modbus_connection.json";
    private const string InputRegistersFileName = "input_registers.json";
    private const string HoldingRegistersFileName = "holding_registers.json";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public ModbusConnectionConfig LoadConnectionConfig()
    {
        string path = BuildConfigPath(ConnectionFileName);

        return LoadJsonFile<ModbusConnectionConfig>(path);
    }

    public IReadOnlyList<ModbusRegisterConfig> LoadInputRegisters()
    {
        string path = BuildConfigPath(InputRegistersFileName);

        return LoadJsonFile<List<ModbusRegisterConfig>>(path);
    }

    public IReadOnlyList<ModbusRegisterConfig> LoadHoldingRegisters()
    {
        string path = BuildConfigPath(HoldingRegistersFileName);

        return LoadJsonFile<List<ModbusRegisterConfig>>(path);
    }

    private static string BuildConfigPath(string fileName)
    {
        return Path.Combine(
            AppContext.BaseDirectory,
            ConfigurationFolderName,
            ModbusFolderName,
            fileName);
    }

    private T LoadJsonFile<T>(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Файл конфигурации не найден: {path}",
                path);
        }

        string json = File.ReadAllText(path);

        T? result = JsonSerializer.Deserialize<T>(json, _jsonOptions);

        if (result is null)
        {
            throw new InvalidOperationException(
                $"Не удалось прочитать JSON-конфигурацию: {path}");
        }

        return result;
    }
}
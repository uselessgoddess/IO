﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class FileHelpers
    {
        /// <summary>
        /// <para>Reads all the text and returns character array from the <paramref name="path"/>.</para>
        /// <para>Читает весь текст и возвращает массив символов из <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file, from which to read the character array.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив символов.</para>
        /// </param>
        /// <returns>
        /// <para>The character array from the <paramref name="path"/>.</para>
        /// <para>Массив символов из <paramref name="path"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] ReadAllChars(string path) => File.ReadAllText(path).ToCharArray();

        /// <summary>
        /// <para>Reads and return all structures from <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает все структуры из <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, from which to read array of <typeparamref name="T"/> structures.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив структур типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры <typeparamref name="T"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadAll<T>(string path)
            where T : struct
        {
            using var reader = File.OpenRead(path);
            return reader.ReadAll<T>();
        }

        /// <summary>
        /// <para>Reads and returns <typeparamref name="T"/> struct from <paramref name="path"/>.</para>
        /// <para>Считывае, и возвращает структуру <typeparamref name="T"/> из <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, from which to read array of <typeparamref name="T"/> structures.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив структур <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para><typeparamref name="T"/> Struct if reading from <paramref name="path"/> was successfull, otherwise default struct.</para>
        /// <para>Структура <typeparamref name="T"/> если чтение с <paramref name="path"/> было успешным, иначе структуру <typeparamref name="T"/> по умолчанию.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadFirstOrDefault<T>(string path)
            where T : struct
        {
            using var fileStream = GetValidFileStreamOrDefault<T>(path);
            return fileStream?.ReadOrDefault<T>() ?? default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FileStream GetValidFileStreamOrDefault<TStruct>(string path) where TStruct : struct => GetValidFileStreamOrDefault(path, Structure<TStruct>.Size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FileStream GetValidFileStreamOrDefault(string path, int elementSize)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            var fileSize = GetSize(path);
            if (fileSize % elementSize != 0)
            {
                throw new InvalidOperationException($"File is not aligned to elements with size {elementSize}.");
            }
            return fileSize > 0 ? File.OpenRead(path) : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadLastOrDefault<T>(string path)
            where T : struct
        {
            var elementSize = Structure<T>.Size;
            using var reader = GetValidFileStreamOrDefault(path, elementSize);
            if (reader == null)
            {
                return default;
            }
            var totalElements = reader.Length / elementSize;
            reader.Position = (totalElements - 1) * elementSize; // Set to last element
            return reader.ReadOrDefault<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFirst<T>(string path, T value)
            where T : struct
        {
            using var writer = File.OpenWrite(path);
            writer.Position = 0;
            writer.Write(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FileStream Append(string path) => File.Open(path, FileMode.Append, FileAccess.Write);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetSize(string path) => File.Exists(path) ? new FileInfo(path).Length : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSize(string path, long size)
        {
            using var fileStream = File.Open(path, FileMode.OpenOrCreate);
            if (fileStream.Length != size)
            {
                fileStream.SetLength(size);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory) => DeleteAll(directory, "*");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern) => DeleteAll(directory, searchPattern, SearchOption.TopDirectoryOnly);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern, SearchOption searchOption)
        {
            foreach (var file in Directory.EnumerateFiles(directory, searchPattern, searchOption))
            {
                File.Delete(file);
            }
        }
    }
}

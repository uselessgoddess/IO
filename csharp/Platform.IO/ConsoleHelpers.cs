﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform.Collections;
using Platform.Collections.Arrays;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class ConsoleHelpers
    {
        /// <summary>
        /// <para>Requests and expects a user to press any key in the console.</para>
        /// <para>Запрашивает и ожидает нажатие любой клавиши пользователем в консоли.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// <para>Gets the argument's value with the specified <paramref name="index" /> from the argument array (<paramref name="args"/>) and if it's absent request input of this value in the console from the user.</para>
        /// <para>Получает значение аргумента с указанным <paramref name="index"/> из массива аргументов <paramref name="args"/>, a если оно отсутствует запрашивает его ввод в консоли у пользователя.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The ordinal number of the argument in the array.</para>
        /// <para>Порядковый номер аргумента в массиве.</para>
        /// </param>
        /// <param name="args">
        /// <para>The argument array passed to the application.</para>
        /// <para>Массив аргументов переданных приложению.</para>
        /// </param>
        /// <returns>
        /// <para>The value with the specified <paramref name="index"/> extracted from the argument array <paramref name="args"/> or entered by a user in the console.</para>
        /// <para>Значение с указанным <paramref name="index"/>, извлечённое из массива <paramref name="args"/>, или введённое пользователем в консоли.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetOrReadArgument(int index, params string[] args) => GetOrReadArgument(index, $"{index + 1} argument", args);

        /// <summary>
        /// <para>Gets the argument's value with the specified <paramref name="index" /> from the argument array (<paramref name="args"/>) and if it's absent request input of this value in the console from the user.</para>
        /// <para>Получает значение аргумента с указанным <paramref name="index"/> из массива аргументов <paramref name="args"/>, a если оно отсутствует запрашивает его ввод в консоли у пользователя.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The ordinal number of the argument in the array.</para>
        /// <para>Порядковый номер аргумента в массиве.</para>
        /// </param>
        /// <param name="readMessage">
        /// <para>The message's text to a user describing which argument is being entered at the moment. If the array <paramref name="args"/> doesn't contain the element with the specified <paramref name="index"/>, then this message is used.</para>
        /// <para>Текст сообщения пользователю описывающее какой аргумент вводится в данный момент. Это сообщение используется только если массив <paramref name="args"/> не содержит аргумента с указанным <paramref name="index"/>.</para>
        /// </param>
        /// <param name="args">
        /// <para>The argument array passed to the application.</para>
        /// <para>Массив аргументов переданных приложению.</para>
        /// </param>
        /// <returns>
        /// <para>The value with the specified <paramref name="index"/> extracted from the argument array <paramref name="args"/> or entered by a user in the console.</para>
        /// <para>Значение с указанным <paramref name="index"/>, извлечённое из массива <paramref name="args"/>, или введённое пользователем в консоли.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetOrReadArgument(int index, string readMessage, params string[] args)
        {
            if (!args.TryGetElement(index, out string result))
            {
                Console.Write($"{readMessage}: ");
                result = Console.ReadLine();
            }
            if (string.IsNullOrEmpty(result))
            {
                return "";
            }
            else
            {
                return result.Trim().TrimSingle('"').Trim();
            }
        }

        /// <summary>
        /// <para>Outputs the <paramref name="string" /> to the console.</para>
        /// <para>Выводит <paramref name="string" /> в консоль.</para>
        /// </summary>
        /// <remarks>
        /// <para>The method is only executed if the application was compiled with the DEBUG directive.</para>
        /// <para>Метод выполняется только в том случае, если приложение было скомпилировано с директивой DEBUG.</para>
        /// </remarks>
        /// <param name="string">
        /// <para>The string to output to the console.</para>
        /// <para>Строка выводимая в консоль.</para>
        /// </param>
        [Conditional("DEBUG")]
        public static void Debug(string @string) => Console.WriteLine(@string);

        /// <summary>
        /// <para>Writes the text representation of the specified object array <paramref name="args"/>, followed by the current line terminator, to the standard output stream using the specified <paramref name="format"/>.</para>
        /// <para>Записывает текстовые представления заданного массива объектов <paramref name="args"/>, за которым следует текущий признак конца строки, в стандартный выходной поток с использованием заданного<paramref name="format"/>.</para>
        /// </summary>
        /// <remarks>
        /// <para>The method is only executed if the application was compiled with the DEBUG directive.</para>
        /// <para>Метод выполняется только в том случае, если приложение было скомпилировано с директивой DEBUG.</para>
        /// </remarks>
        /// <param name="format">
        /// <para>The composite format string.</para>
        /// <para>Строка составного формата.</para>
        /// </param>
        /// <param name="args">
        /// <para>The pbject array to write to the standard output stream using <paramref name="format" />.</para>
        /// <para>Массив объектов для записи в стандартный выходной поток с использованием <paramref name="format" />.</para>
        /// </param>
        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args) => Console.WriteLine(format, args);
    }
}

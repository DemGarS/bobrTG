using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var client = new TelegramBotClient(" ");
        client.StartReceiving(UpdateHandler, Error); /*метод, который выводит бот*/
        Console.ReadLine();
    }
    private static async Task MessageHeandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                {
                    var message = update.Message;
                    var user = message.From;
                    var chat = message.Chat;

                    switch (message.Type)
                    {
                        case MessageType.Text:
                            {
                                if (message.Text == null)
                                {
                                    return;

                                }
                                if (message.Text == "/start")
                                {
                                    //создание кнопок в строке
                                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                                    {
                                        new []
                                        {
                                            InlineKeyboardButton.WithCallbackData(text: "Обо мне", callbackData: "11"),
                                            InlineKeyboardButton.WithCallbackData(text: "Звук бобра???", callbackData: "12"),
                                        },
                                        new []
                                        {
                                            InlineKeyboardButton.WithCallbackData(text: "А ты бобер?", callbackData: "21"),

                                        },
                                        });

                                    Message sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: chat.Id,
                                        text: "Cześć, wybierz, co chcesz zrobić",
                                        replyMarkup: inlineKeyboard,
                                        cancellationToken: cancellationToken);

                                    return;
                                }
                                return;
                            }
                    }
                    return;
                }
        }

    }
    private static async Task CallBack(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //вывод в зависимости от выбранной кнопки
        if (update != null && update.CallbackQuery != null)
        {
            string answer = update.CallbackQuery.Data;
            switch (answer)
            {
                case "11":
                    await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Привет,Я бот ,который помогает определится с самоопеределением , ответь на вопрос и реши являешься ли ты бобром");
                    break;
                case "12":
                    await LoadVideo(botClient, update, cancellationToken);
                    break;
                case "21":

                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        new[]
                        {
                                            InlineKeyboardButton.WithCallbackData(text: "Да", callbackData: "22"),

                                            InlineKeyboardButton.WithCallbackData(text: "Нет, я не ты", callbackData: "23"),
                                        }
                        });
                    Message sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: update.CallbackQuery.Message.Chat.Id,
                                        text: "Ты бобер???",
                                        replyMarkup: inlineKeyboard,
                                        cancellationToken: cancellationToken);


                    break;
                case "22":
                    await botClient.SendPhotoAsync(
    chatId: update.CallbackQuery.Message.Chat.Id,
    photo: InputFile.FromUri("https://i.imgur.com/LFgt88g.png"),
    caption: "<b>Теперь ты гражданин Бобруйска</b>",
    parseMode: ParseMode.Html,
    cancellationToken: cancellationToken);

                    break;
                case "23":
                    await botClient.SendPhotoAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
    photo: InputFile.FromUri("https://sun9-75.userapi.com/impf/xIriEnm2s0xb1rS_-ioOkpFV9Dm_eh2aHEX5CA/odyCIw-pAbE.jpg?size=545x604&quality=95&sign=7b06aa91516aa8c78d7b07d28c6e0b24&type=album"),
    caption: "<b>Че ты тогда тут забыл?</b>",
    parseMode: ParseMode.Html,
    cancellationToken: cancellationToken);
                    break;

            }



            //InlineKeyboardMarkup inlineKeyboard = update.CallbackQuery.Message.ReplyMarkup!;
            //var inlines = inlineKeyboard.InlineKeyboard;
            //foreach (var item1 in inlines)
            //{
            //    foreach (var item2 in item1)
            //    {

            //       
            //    }
            //}
        }
    }
    private static async Task LoadVideo(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Stream stream = System.IO.File.OpenRead("VideoResourses/videoplayback.mp4");

        Message message = await botClient.SendVideoNoteAsync(
            chatId: update.CallbackQuery.Message.Chat.Id,
            videoNote: InputFile.FromStream(stream),
            duration: 47,
            length: 360, // value of width/height
            cancellationToken: cancellationToken);
    }
    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await CallBack(botClient, update, cancellationToken);
        await MessageHeandler(botClient, update, cancellationToken);

    }
}
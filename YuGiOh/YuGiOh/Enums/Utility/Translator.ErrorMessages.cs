using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
        #region Properties
    /// <summary>
    /// Contains <see cref="ErrorMessage"/> translations for every <see cref="Language"/>
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<ErrorMessage, string>> Errors { get; } = new
    (
        new Dictionary<Language, IDictionary<ErrorMessage, string>>
        {
            {
                Language.en, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "Error" },
                    { ErrorMessage.Description, "Card is missing, please download/update your card list" }
                }
            },
            {
                Language.de, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "Fehler" },
                    { ErrorMessage.Description, "Karte fehlt, bitte laden/aktualisieren Sie Ihre Kartenliste" }
                }
            },
            {
                Language.es, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "Error" },
                    { ErrorMessage.Description, "Falta la tarjeta, descargue/actualice su lista de tarjetas" }
                }
            },
            {
                Language.fr, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "Erreur" },
                    { ErrorMessage.Description, "La carte est manquante, veuillez télécharger/mettre à jour votre liste de cartes" }
                }
            },
            {
                Language.it, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "Errore" },
                    { ErrorMessage.Description, "Carta mancante, scaricare/aggiornare l'elenco delle carte" }
                }
            },
            {
                Language.ja, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "エラー" },
                    { ErrorMessage.Description, "カードが見つかりません。カード リストをダウンロード/更新してください" }
                }
            },
            {
                Language.ko, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "오류" },
                    { ErrorMessage.Description, "카드가 없습니다. 카드 목록을 다운로드/업데이트하세요" }
                }
            },
            {
                Language.pt, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "Erro" },
                    { ErrorMessage.Description, "Falta o cartão, faça o download/atualize sua lista de cartões" }
                }
            },
            {
                Language.zh, new Dictionary<ErrorMessage, string>
                {
                    { ErrorMessage.Name, "错误" },
                    { ErrorMessage.Description, "卡丢失，请下载/更新您的卡列表" }
                }
            }
        }
    );
    #endregion
}
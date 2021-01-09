export class MessagesDto {
    /// <summary>
    /// İşlem başarılı
    /// - Status Code: Ok
    /// </summary>
    public static Ok = "Ok";

    /// <summary>
    /// Kayıt bulunamadı 
    /// - Status Code: NotFound
    /// </summary>
    public static GNE0001 = "GNE0001";

    /// <summary>
    ///  Kullanıcıya mail gönderilemedi
    /// - Status Code: InternalServerError
    /// </summary>
    public static GNE0002 = "GNE0002";

    /// <summary>
    ///  Bu email ile daha önce bir kayıt oluşturulmuş.
    /// - Status Code: BadRequest
    /// </summary>
    public static GNE0003 = "GNE0003";

    /// <summary>
    ///  Kayıt sırasında hata oluştu.
    /// - Status Code: BadRequest
    /// </summary>
    public static GNE0004 = "GNE0004";

    /// <summary>
    ///  Beklenmedik bir hata oluştu.
    /// - Status Code: BadRequest
    /// </summary>
    public static GNE0005 = "GNE0005";


}
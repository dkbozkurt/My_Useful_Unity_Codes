// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

/// <summary>
/// Interfaceler siniflari belirli bir biçime sokmak için ortaya çıkmış yapılardır.
/// C# ve Java gibi dillerde bir sınıf sadece bir adet üst sınıftan oluşturulabileceği için bu gibi durumlarda interfaceler
/// kullanilir.
///
/// Interfacelerin kendi objeleri yoktur. Sadece bir sinifa implement edilebilirler.
///
/// Interfaceler tek başına hiç bir işlev yapamaz. Kendi içlerinde değişkenler, fieldler tutamaz.
/// Interfaceler sadece methodlar ve propertiler tutabilir ve bunlarin içi boş olmak zorundadır.
///
/// Tek amaci implement edilen sinifa, benim içimde bu fonksiyonlarım var senin içinde de bu fonksiyonlar olmak zorunda
/// demektir.
///
/// Interfacelerin ilk harfi büyük ı ile başlar.
///
/// Objelerimiz damage alabilsin ve ölebilsin diye "IDamageable" ve "IKillable" adinda interfaceler olusturduk.
///
/// Interfacenin yanina generic type verip onu implement edeceğimiz classta türünü belirtirsek bu sayede ister nit ister float
/// etc. seklinte uygulamaya koyabiliriz.
/// 
/// Ref : https://www.youtube.com/watch?v=nFcYxYdBgrw
/// </summary>


public interface IDamageable<T>
{
    // Property
    T Health { get; set; }
    
    // Method
    void Damage(T damageTaken);
}

public interface IKillable
{ 
    void Kill();
}

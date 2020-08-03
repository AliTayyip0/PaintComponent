# C#, picturebox için Çizim Komponenti

Bu component sayesinde picturebox üzerinde çizim işlemleri gerçekleştirebileceksiniz.


# Kurulum

PaintComponent.cs uzantılı dosyayı indirdikten sonra c# projenizde add new Existing Item ile dosyayı projenize dahil edin. Projenize dosyayı ekledikten sonra solution kısmında namespace kısmına sağ tıklayarak sırasıyla build ve rebuild işlemlerini yapın.
Bu işlemler sonucunda componentin toolbox'ınıza geldiğini göreceksiniz.

![image](https://user-images.githubusercontent.com/28540685/89153745-2c070d80-d56e-11ea-8f34-6a0f90e36aea.png)

Componenti kullanmak istediğiniz Form'a gelin ve componenti formun üstüne sürükleyin.  
Component alt kısımda görülecektir. 

![image](https://user-images.githubusercontent.com/28540685/89153495-a8e5b780-d56d-11ea-9776-492a881f0b94.png)

Componentin üstüne tıklayıp Property kısmında hangi picturebox için kullanacağınızı seçin.

![image](https://user-images.githubusercontent.com/28540685/89153677-07129a80-d56e-11ea-994d-04644eff2345.png)

Picturebox seçimini gerçekleştirdikten sonra çizim sürünü işaretlemeniz gerekmektedir. Bunun için CizimTuruDegistirme isimli fonksiyonu kullanmanız gerekir. Bu fonksiyon yardımıyla kalemi işaretledikten sonra picturebox üzerinde herhangi bir noktaya tıkladığınızda çizim işlemi yapılmaya başlanır.

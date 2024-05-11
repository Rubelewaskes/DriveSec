# DriveSec
Защищённый файлообменник для стартапчика
## Настрйока докера в первый раз
1. [Скачайте и установите докер] (https://www.docker.com/products/docker-desktop/)
2. Откройте командную строку и поочереди введите 3 команды:
   ```shell
   cd путь/к/папке/с/докерфайлом/и/бекапом
   docker build -t my-postgres-container .
   docker run -p 5438:5432 --name dbDS -d my-postgres-container
   ```
   В результате должен запуститься контейнер с базой данных. Это можно проверить зайдя в приложение во вкладку Containers, если контейнер есть и он залёный, то всё ок, если нет, то не пишите и не звоните мне.
3. Теперь можно добавить сервер БД в pgAdmin, для этого надо выбрать Register -> Server.
4. В открывшемся окне ввести имя dbDS, во вкладке connection Host/address: localhost, Port:5438, username:postgres, password:1234.
5. Если всё успешно, то можно пользоваться, в пг менять данные, запускать приложение и т. д.

**ЕСЛИ ВНОСИТЕ ИЗМЕНЕНИЯ В БД, ТО НЕ ЗАБЫВАЙТЕ ОБНОВИТЬ БЕКАП**

По хорошему, надо так запускать всегда, но дальше можно **просто открывать приложение и включать докер**, поэтому всегда уведомляйте, если изменили что-то в бд.

## Создание бекапа
Когда создаёте бекап:
1. Используйте имя DS_Backup.sql
2. Format ставьте Plain
3. **НЕ НАДО БЕКАПОВ СЕРВЕРОВ** 👊

## Работа с SCSS
**Если у файла расширение не .SCSS можно не делать следующие шаги**
Теперь у нас подключен препроцессор SCSS, поэтому чтобы работать с CSS нужно сделать следующее:
1. Установить расширение Web Compiler 2022+ и перезапустить студию
2. После изменения файла .SCSS необходимо нажать правой кнопкой мыши в обозревателе решений -> Web Compiler -> Re-compile file **ИНАЧЕ ИЗМЕНЕНИЯ НЕ СОХРАНЯТСЯ**
3. Если хотите создать новый .SCSS - в первые две строчки нужно прописать следующее:
   ```shell
   @import "../lib/bootstrap/dist/scss/_functions.scss";
   @import "../lib/bootstrap/dist/scss/_variables.scss";

   // Остальной CSS-код
   ```
4. Сделать тоже самое, что в пункте 2
5. При необходимости в файле compilerconfig.json можно поменять код (думаю объяснения тут не нужны)
   ```shell
   [
      {
       "outputFile": "wwwroot/css/site.css",
       "inputFile": "wwwroot/css/site.scss"
     }
   ] 
   ```

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using PuppeteerSharp;

namespace OracleScraper
{
    class DoList
    {
        public async static Task<string> Action(Page page, PathFile filePath, string dateStr)
        {
            try
            {

                var element = await page.WaitForXPathAsync("/html/body/table[1]/tbody/tr[2]/td/div/form/table/tbody/tr/td/table/tbody/tr[3]/td[3]/div/input"); 

                await element.TypeAsync("User");

                element = await page.WaitForXPathAsync("/html/body/table[1]/tbody/tr[2]/td/div/form/table/tbody/tr/td/table/tbody/tr[5]/td[3]/div/input");

                await element.TypeAsync("Password");

                element = await page.WaitForXPathAsync("//*[@id='loginForm']/table/tbody/tr/td/table/tbody/tr[6]/td[3]/div/input");

                await element.ClickAsync();


                await page.WaitForTimeoutAsync(4000);

                ///Первая страница после регистрации

                //Загрузка по ПУ RadioButton
                element = await page.WaitForXPathAsync("/html/body/center[1]/table/tbody/tr/td/table/tbody/tr[10]/td[2]/input");
                await element.ClickAsync();

                element = await page.WaitForXPathAsync("/html/body/center[1]/table/tbody/tr/td/table/tbody/tr[10]/td[3]/select");
                await element.ClickAsync();

                //for (int i = 0; i < 9; i ++)
                //{
                //    await page.Keyboard.DownAsync("ArrowDown");                       
                //}
                //await page.Keyboard.PressAsync("Enter");

                element = await page.WaitForXPathAsync("/html/body/center[1]/table/tbody/tr/td/table/tbody/tr[15]/td/div/input");
                await element.ClickAsync();

                await page.WaitForTimeoutAsync(4000);

                ////////////////////////////
                ///

                ///Вторая страница Закгрзки Интерфейса

                //Филиал
                element = await page.WaitForXPathAsync("//*[@id='drdw_CISDIV']");
                await element.ClickAsync();

                for (int i = 0; i < 3; i++)
                {
                    await page.Keyboard.DownAsync("ArrowDown");
                }
                await page.Keyboard.PressAsync("Enter");


                //Файл
                element = await page.WaitForXPathAsync("//*[@id='file_FILE']");
                await element.ClickAsync();               
                    await page.WaitForTimeoutAsync(1000);
                KeyBoard($"{filePath.Path}");

                //Источник ППУ
                element = await page.WaitForXPathAsync("//*[@id='drdw_SOURCE_CD']");
                //TODO: Дописать все возможные вариации для выборки источника ППУ исходя из названия файла!!!
                var array = await element.QuerySelectorAllAsync("option");

                var ss = await array[0].EvaluateFunctionAsync<string>("e=> e.value");

                await element.SelectAsync($"{filePath.SettingUpload}"); /// Внимание на пробел!!!!!!!!!  option в выпадающем списке с пробелом, без него не выбирает элемент                   


                  ///ДАта
                element = await page.WaitForXPathAsync("//*[@id='dprd_PERIOD']");  ///  
                await element.TypeAsync($"{dateStr}");

                for (int i = 0; i < 7; i++)
                {
                    await page.Keyboard.DownAsync("Delete");
                }

                //Максимальное количество ошибок
                element = await page.WaitForXPathAsync("//*[@id='nmbr_MAXERROR']");  ///  //*[@id="nmbr_MAXERROR"]
                await element.TypeAsync("10000000000");

                for (int i = 0; i < 4; i++)
                {
                    await page.Keyboard.DownAsync("Delete");
                }


                element = await page.WaitForXPathAsync("//*[@id='drdw_PUSRCH']");
                await element.ClickAsync();
                await page.Keyboard.DownAsync("ArrowDown");
                await page.Keyboard.PressAsync("Enter");




                //Выполнить
                element = await page.WaitForXPathAsync("/html/body/center/table/tbody/tr/td/table/tbody/tr[12]/td/div/input");
                //TODO: Разкоментировать строку что бы программа нажала кнопку "ВЫПОЛНИТЬ"
                //await element.ClickAsync();


                ///TODO: Получить элемент после нажатия кнопки выполнить
                ///
                //element = await page.WaitForXPathAsync("/html/body/center/table/tbody/tr/td/table/tbody/tr[12]/td/div/input");
                await page.WaitForNavigationAsync(new NavigationOptions { Timeout = 5000000});

                await page.WaitForTimeoutAsync(3000);

                return $"Файл {filePath.Path} загружен";

                
            }
            catch(Exception ex)
            {
                return $"Файл {filePath.Path} Ошибка загрузки \n" + ex.ToString();
            }
        }
        public static void KeyBoard(string Text)
        {
            InputSimulator Simulator = new InputSimulator();
            Simulator.Keyboard.TextEntry($"{Text}");
            Simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Simulator.Keyboard.Sleep(1000);
        }
    }

}

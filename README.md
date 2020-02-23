<h1>Документация к проекту WebAppBook</h1>
<p>Данный   проект нацелен на ведение учета книг (СУБД). За его реализацию мною было принято взяться после завершения <a href="https://github.com/Victor954/-AtelierWebApplication">прошлого проекта</a> и получением более глубоких знаний в ООП и C#.</p>
<p><b>ВНИМАНИЕ!</b> Данная документация не является точным описанием всего функционала, по причине слишком большого объема компонентов. Но ее прочтение поможет легче влиться во внутреннее устройство приложения.</p> 
</br>
<h2>Краткое описание структуры проекта:</h2>
<p><b>Контроллеры</b> – являются точкой инициализации или наследуются от классов Base_Class.</p>
<p><b>Классы пространства имен Base_Class</b> сочетают в себе обработку данных и обращение к БД.</p>
<p><b>Модуль</b> - содержит классы таблиц для EF и еще класс ListCollection.</p>
<p><b>View</b> – разделяется на 3 категории:</p>
<ul>
<li>Представления для таблиц (Контроллеры Book, Author и Publisher).Каждый контроллер содержит представления для вывода таблицы(Имя контроллера) ,изменения(EditForm) и добавления(AddForm).</li>
<li>Базовое представление Index.</li>
<li>Иные подключаемые представления (Меню, Layout, Пагинация и.т.д).</li>
</ul>
</br>
<h2>Контроллеры:</h2>
<p>Контроллеры (за исключением HomeController) находятся в папках Filter , MainTable , SecondTable , что делит их на следующие категории:</p>
<p><b>Filter</b> – содержит контроллеры для фильтрации.</p>
<p><b>MainTable</b> – содержит контроллеры для команд CRUD больших по размеру таблиц (Book, Author и Publisher).</p>
<p><b>SecondTable</b> – содержит контроллеры для команд CRUD малых по размеру таблиц.</p>
<p>Ниже более подробно рассматривается каждая из категорий и контроллер Home.</p>
<p><h3>1.Filter:</h3></p>
<p><code>AuthorSearchController</code> – контроллер для фильтрации коллекции объектов таблицы <code>Author</code>. Как и все контроллеры в Filter он наследуется от <code>FilterController<T></code> с указанным типом фильтруемых объектов, где в свою очередь создается базовый класс фильтрации <code>FilterContext<T></code>, обрабатывающий класс <code>ConcreteFilter<T></code> через метод <code>SetFilterResult.ConcreteFilter<T></code> с указанным типом фильтруемых объектов служит набором определённых фильтров и имеет методы фильтрации, что и вызываются в action методах.Во время обращения запроса к action методу происходит перенаправление к action <code>TableInvoke</code> контроллера с именем соответствующим фильтруемому типу.</p>
<p><code>BookSearchController</code> и <code>PublisherSearchController</code> почти аналогичны <code>AuthorSearchController</code>, только используют другие типы для фильтрации и чуть иначе выполняют обработку запроса на фильтрацию.</p>
<p><h3>2.MainTable:</h3></p>
<p>Все контроллеры наследуются от <code>TablesBaseDataController<T></code> с указанным типом обрабатываемого объекта таблицы, где определяются следующие методы и поля:</p>
<p><code>AddWithFileCommand<T> tableAndFileControll</code> – используется для сохранения картинки и таблицы.</p>
<p><code>protected static ControllerInfo<T></code> container – контейнер значений , что являются уникальными для каждого контроллера.</p>
<p><code>protected abstract ControllerInfo<T> SetContainerValue()</code> – абстрактный метод для инициализации  ControllerInfo<T> значениями.
<p><code>public string Upload()</code> – добавляет путь файла и сохраняет его в виде byte массива.
<p><code>public void RemoveTable(T table)</code> – удаляет поле равное переданному объекту T table  из таблицы.
<p><code>public ActionResult TableInvoke()</code> – возвращает представление для вывода на страницу , использует либо данные из фильтрации , либо делает запрос на их выборку
<p><code>public ActionResult AddForm()</code> – возвращает представление для добавления поля в таблицу.
<p><code>public ActionResult EditForm(int id)</code>  - возвращает представление настройки выбранного поля. 
<p><code>public ActionResult SetPagination(ushort page)</code> – action для настройки страницы пагинации , переопределяет в TableInvoke() для  вывода нового результата.
<p>Все контроллеры в MainTable имеют данные поля и методы. По мимо этого в них еще есть <code>AddTable()</code> и <code>EditTable()</code> , для изменения поля в таблице или ее добавления.
<p><h3>3. SecondTable:</h3></p>
<p>Все контроллеры наследуются от <code>TablesSecondDataController</code> с указанным типом обрабатываемого объекта таблицы, где определяются следующие методы и поля:
<p><code>protected static CommandContext selectContext</code> – позволяет делать запросы, возвращающие коллекцию объектов таблиц .
<p><code>protected static CommandVoidContext voidContext</code> – позволяет делать запросы ничего не возвращающие.
<p><code>static SearchTableFromContext<T> dbSetType</code> – по умолчанию принимает метод ,что в зависимости от указанного типа позволяет манипулировать с ним подобной таблицей.
<p><code>protected List<T> listSecondTables</code> – сюда передается лист из ListCollection для манипуляции над ним через action методы.
<p><code>protected Search<T> searchMethod</code> – хранит  метод определяющий тип таблицы в БД.
<p><code>private JsonResult SetResulst()</code> – используется для создания  CommandVoidContext команд и возврата.
<p><code>public void AddList()</code> – добавляет в ListCollection переданный объект.
<p><code>public void RemoveList()</code> – удаляет из  ListCollection переданный объект.
<p><code>public JsonResult AddDb()</code> – добавляет в бд переданный объект.
<p><code>public JsonResult EditDb()</code> – изменяет  в бд переданный объект.
<p><code>public JsonResult DeleteDb()</code> – удаляет в бд переданный объект.
<p><code>public virtual JsonResult Search()</code> – виртуальный метод. Производит поиск по переданной строке, может быть переопределен и создана новая реализация поиска. 
<p>Все контроллеры MainTable определяют через конструктор все необходимые для работы поля.
<p><h3>4.HomeController</h3></p>
<p>Наследуется от <code>TablesData</code> и определяет action для index , где вызывается <code>TablesData.View()</code> с переданными значениями размера установленных <code>ViewBag</code>.Сбрасываются значения <code>ListCollection</code> и устанавливается страница пагинации на 0.</p>
</br>
<h2>Описание классов, делегатов, интерфейсов и перечислений:</h2>
<p><h3>1.CommandCRUD.cs<p></h3>
<p><b>Интерфейсы:</b>
<p><code>ICommandVoid</code> – может быть наследован и использован в <code>CommandVoidContext</code> для создания запроса не возвращающего значение.
<p><code>ICommandResult<T></code> – может быть наследован и использован в <code>CommandContext</code> для создания запроса, возвращающего <code>IEnumerable</code> полей таблиц T. Так же <code>CommandContext</code> поддерживает асинхронный вариант <code>ICommandResult</code> - <code>IСommandResultAsync</code>.
<p><code>IСommandResultAsync<T></code> – может быть наследован и использован в <code>CommandContext</code> для создания асинхронного запроса возвращающий <code>List</code> таблиц T.
<p><code>ICommandSingle<T></code> – может быть наследован и использован в <code>CommandSingleContext</code> для создания запроса возвращающий одно поле таблицы в виде объекта таблицы T.
<p><code>ITableInclude<T></code> - может быть наследован для создания класса управляющего над связанными таблицами.
<p><code>IFileCommand<T></code> - может быть наследован для создания класса управляющего над файлами.
<p><b>Классы:</b>
<p><code>CommandContext</code> - Singleton класс. Создает подключение к бд и передает в <code>ICommandResult<T></code> или <code>IСommandResultAsync<T></code> контекст EF, возвращая результат запроса. Имеет в себе методы часто вызывающих запросов.
<p><code>CommandVoidContext</code> – схож с <code>CommandContext</code> , только принимает  <code>ICommandVoid</code> и имеет методы самых частых ничего не возвращаемых запросов.
<p><code>CommandSingleContext</code> - схож  с <code>CommandContext</code> , только принимает  <code>ICommandSingle<T></code> и имеет методы самых частых возвращающих только один объект таблицы запросов.
<p><b>Делегаты:</b>
<p>SearchTableFromContext <T> - использует для определения таблицы из контекста.
<p>IncludeTables<T,A> - используется для синхронизации объекта таблицы с ее привязанными объектами таблиц. 
<p><h3>2.ConcreteCRUD.cs</p></h3>
<p><b>Классы:</p></b>
<p><code>AddCommand<T></code> - наследуется от <code>ICommandVoid</code> и в зависимости от указанного типа с соответствующей таблицей будут происходить манипуляции указанные в <code>EntityState</code>.
<p><code>BaseSelectCommad<T></code> - частичный класс, предоставляющий общий интерфейс для создания классов унаследованных от <code>ICommandResult<T></code> или <code>ICommandResultAsync<T></code>.
<p><code>SelectCommand<T></code> - по переданному параметру выборки возвращает либо пагинированый результат, либо все элементы сразу.
<p><code>SelectCommandAsycn<T></code> - асинхронная версия <code>SelectCommand<T></code>.
<p><code>SingleController <T></code> - по переданному определению таблицы и <code>id</code> ищет через отражение таблицу с этим <code>id</code> в EF и возвращает ее объект.
<p><code>EditManyToManyCommand <T></code> - создает <code>AddManyToManyCommand<T></code> и передает туда первый элемент по переданному <code>id</code>.
<p><code>AddManyToManyCommand</code> – добавляет связанные таблицы к главной таблице.
<p><code>RelatedTableCommand <A, T></code> – позволят управлять связанными объектами-таблиц типа T для таблицы типа A.
<p><code>AddWithFileCommand<T></code>  - сохраняет  путь и файл в виде байт массива для передачи их в объект класса реализующий <code>IFileCommand<T></code> ,выполняющий управление с ними.
<p><code>DefaultEditFile<T></code> - сохраняет или добавляет указанный объект таблицы вместе с файлом, если тот не равен <code>null</code>.
<p><code>DefaultControllFile<T></code> - сохраняет файл по переданному пути и вместе с ним выполняет <code>EntityState</code> для переданного объекта таблицы. 
<p><h3>3.CommandFilter.cs</p></h3>
<p><b>Классы:</p></b>
<p><code>CollectionFilterCommad <T></code> - управляет цепочкой возвращаемых фильтрами значений и является суперклассом для всех конкретных реализаций фильтра.
<p><code>FilterName<T></code> - выполняет фильтрацию по строке и передает результат в цепочку <code>CollectionFilterCommad<T></code>.
<p><code>FilterMayny<T,A></code> - производит поиск по связанным таблицам и передает результат в цепочку <code>CollectionFilterCommad<T></code>.
<p><code>FilterSelectController<T></code> - содержит лист фильтров и изначальных данных для фильтрации, позволяет добавлять новые фильтры и вызывать их обработку с возвратом последующего результата. Применяется пагинация
<p><b>Делегаты:</p></b>
<p><code>StringFormater <T></code> - конвертирует переданный объект таблицы в строку.
<p><h3>4.FilterController.cs</p></h3>
<p><b>Классы:</p></b>
<p><code>FilterController<T></code> - базовый контроллер, содержащий класс <code>FilterContext<T></code>, что при фильтрации принимает класс конкретного набора фильтров.
<p><h3>5.FilterTable.cs</p></h3>
<p><b>Интерфейсы:</p></b>
<p><code>IFilterPart<T></code> - определяет интерфейс для классов, содержащих правила фильтрации. Возвращает лист фильтров.
<p><b>Классы:</p></b>
<p><code>ConcreteFilter<T></code> - конкретная реализация правила фильтраций. Возвращает лист фильтров равный правилам фильтрации. 
<p><code>FilterContext<T></code> - класс, вызывающий переданные классы правил фильтров, передавая в качестве начального значения <code>IQueryable</code> запроса таблиц типов T.
<p><h3>6.InterimTypes.cs</p></h3>
<p><b>Классы:</p></b>
<p><code>ReflectionEfId<T></code> - статический класс с одним методом, что возвращает <code>Id</code> переданного объекта T при помощи отражения.
<p><code>Pagination</code> – статический  класс пагинации . Позволяет по переданному <code>IQueryable</code> запросу получить от туда <code>takeCount</code> элементов пропуская объекты относительно номера страницы <code>startPoint</code> и количества <code>takeCount</code>. Вычисляет количество страниц при помощи <code>SetPageSize()</code>.
<p><code>PanigationFilter</code> – кэширует изначальный <code>IQueryable</code> запрос для повторной пагинации фильтрации.
<p><code>SelectDefault</code> – набор часто используемых значений делегата <code>SearchTableFromContext</code>.
<p><h3>7.OperationCommand.cs</p></h3>
<p><b>Перечисления:</p></b>
<p><code>CommandType</code> – предоставляет выбор либо добавления таблицы, либо ее изменения.
<p><b>Классы:</p></b>
<p><code>TableOperationContext<T></code> - абстрактный класс для предоставления метода создающий в зависимости от значения <code>CommandType</code> либо <code>EditManyToManyCommand<T></code>, либо <code>AddManyToManyCommand<T></code> соответствующего типа T.
<p><code>BookOperation</code> – класс для манипуляции с объектом таблицы <code>Book</code>.Создает все необходимые значения и передает их в свой суперкласс <code>TableOperationContext<T></code> при помощи <code>GetCommand()</code>.
<p><code>BookOperation</code> – класс для манипуляции с объектом таблицы <code>Author</code>.Создает все необходимые значения и передает их в свой суперкласс <code>TableOperationContext<T></code> при помощи <code>GetCommand()</code>.
<p><h3>8.TablesDataController.cs</p></h3>
<p><b>Перечисления:</p></b>
<p><code>SizeSendViewBag</code> – предоставляет выбор размера загрузки <code>ViewBag</code> с различными коллекциями объектов таблиц.
<p><b>Классы:</p></b>
<p><code>TablesSecondDataController<T></code> - абстрактный контроллер для предоставления методов CRUD малым по размеру таблицам.  
<p><code>TablesData</code> – частичный контроллер для передачи всех контекстов работы с БД и методов возвращающих <code>ViewBag</code> в зависимости от значения <code>SizeSendViewBag</code>.
<p><code>TablesBaseDataController<T></code> - абстрактный контроллер предоставления методов CRUD крупным по размеру таблицам. Поддерживается пагинация и вариативность значений через структуру <code>ControllerInfo</code>.
<p><b>Структуры:</p></b>
<p><code>ControllerInfo</code> – содержит readonly поля для определения у наследуемых от <code>TablesBaseDataController<T></code> параметров .
<p><b>Делегаты:</p></b>
<p><code>Search<T></code> - позволяет вычленить из контекста БД таблицы с полями соответствующими переданной строке
<p><h3>9.ListCollection.cs</p></h3>
<p><b>Классы:</p></b>
<p><code>ListCollection</code> – содержит промежуточные листы для всех малых по размеру таблиц. Данное решение (костыль) было принято чтобы данные не добавлялись сразу в БД , а предварительно находились в виде коллекций. Так это упрощает добавление категорий в большие таблицы. 

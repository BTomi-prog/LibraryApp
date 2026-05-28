using LibraryApp;

namespace LibraryAppTests
{
    [TestClass]
    public class Tests
    {
        private Library CreateDefaultLibrary()
        {
            var lib = new Library("City Library");
            lib.AddBook("Dune", 3);
            lib.AddBook("1984", 1);
            return lib;
        }
        private Library CreateZeroLibrary()
        {
            var lib = new Library("City Librar");
            return lib;
        }

        // ---- Constructor ----

        [TestMethod]
        public void Constructor_ValidName()
        {
            var lib = new Library("City Library");
            Assert.AreEqual("City Library", lib.GetName());
        }
        // TODO: null vagy üres névvel létrehozva ArgumentException-t kell dobni
        

        [TestMethod]
        public void Constructor_EmptyName()
        {
            Assert.ThrowsException<ArgumentException>(() => new Library(""));
        }

        // ---- AddBook ----

        [TestMethod]
        public void AddBook_NewTitle()
        {
            var lib = new Library("City Library");
            lib.AddBook("Dune", 2);
            Assert.AreEqual(1, lib.GetTotalTitles());
        }
        // TODO: ugyanazt a címet hozzáadva újabb bejegyzések kerülnek az _availableBooks listába, és GetTotalTitles nem változik
        [TestMethod]
        public void AddBook_ExistingTitle()
        {
            var lib = CreateDefaultLibrary(); 
            lib.AddBook("Dune", 2);
            Assert.AreEqual(1, lib.GetTotalTitles());
            Assert.AreEqual(5, lib.GetAvailableCopies("Dune"));
        }

        // TODO: copies értéke 0 vagy negatív esetén ArgumentException-t kell dobni
        [TestMethod]
        public void AddBook_InvalidCopies()
        {
            var lib = new Library("City Library");
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", 0));
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", -1));
        }

        // ---- BorrowBook ----

        [TestMethod]
        public void BorrowBook_AvailableCopy()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            bool result = lib.BorrowBook("Dune");
            Assert.IsTrue(result);
            Assert.AreEqual(2, lib.GetAvailableCopies("Dune"));
        }
        // TODO: nem létező cím esetén false-t kell visszaadni és nem dob kivételt
        [TestMethod]
        public void BorrowBook_NonExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.BorrowBook("NonExistingTitle");
            Assert.IsFalse(result);
        }
        // TODO: az összes példány kikölcsönzése után újabb kölcsönzés false-t ad vissza
        [TestMethod]
        public void BorrowBook_ExistingTitle() 
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.BorrowBook("1984");
            Assert.IsFalse(result);
        }

        // ---- ReturnBook ----

        [TestMethod]
        public void ReturnBook_BorrowedCopy()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("1984");
            bool result = lib.ReturnBook("1984");
            Assert.IsTrue(result);
            Assert.AreEqual(1, lib.GetAvailableCopies("1984"));
        }
        // TODO: nem létező cím visszahozásakor false-t kell visszaadni
        [TestMethod]
        public void Return_NotExesting()
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.ReturnBook("aiudshasd");
            Assert.IsFalse(result);
        }
        // TODO: olyan könyv visszahozásakor, amelyből semmi sincs kikölcsönzve, false-t kell adni
        [TestMethod]
        public void Return_NotBorrowed()
        {
            var lib = CreateDefaultLibrary();
            bool result = lib.ReturnBook("Dune");
            Assert.IsFalse(result);
        }




        // ---- GetAvailableCopies ----

        [TestMethod]
        public void GetAvailableCopies_AfterBorrow()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            lib.BorrowBook("Dune");
            lib.BorrowBook("Dune");
            Assert.AreEqual(1, lib.GetAvailableCopies("Dune"));
        }
        // TODO: nem létező cím esetén -1-et kell visszaadni
        [TestMethod]
        public void GetAvailableCopies_NotExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("Dune");
            lib.BorrowBook("Dune");
            Assert.AreEqual(1, lib.GetAvailableCopies("Dune"));
        }

        // ---- IsAvailable ----

        [TestMethod]
        public void IsAvailable_BookWithFreeCopies()
        {
            var lib = CreateDefaultLibrary();
            Assert.IsTrue(lib.IsAvailable("Dune"));
        }
        // TODO: teljesen kikölcsönzött könyv esetén false-t kell visszaadni
        [TestMethod]
        public void IsAvailable_EveryThing()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("Dune");
            
            Assert.IsTrue(lib.IsAvailable("Dune"));
        }


        // TODO: nem létező cím esetén false-t kell visszaadni

        // ---- GetTotalBorrowed ----

        [TestMethod]
        public void GetTotalBorrowed_AfterMultipleBorrows()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("Dune");
            lib.BorrowBook("1984");
            Assert.AreEqual(2, lib.GetTotalBorrowed());
        }
        // TODO: újonnan létrehozott, üres könyvtárban GetTotalBorrowed() nullát ad vissza
        [TestMethod]
        public void GetTotalBorrowed_NewBook()
        {
            var lib = CreateZeroLibrary();
            lib.BorrowBook("Dune");
            lib.ReturnBook("Dune");

            Assert.AreEqual(0, lib.GetTotalBorrowed());
        }
        // TODO: visszahozás után a kikölcsönzött darabszám helyesen csökken

        // ---- RemoveBook ----

        [TestMethod]
        public void RemoveBook_ExistingTitle()
        {
            var lib = CreateDefaultLibrary(); // 2 cím
            bool result = lib.RemoveBook("1984");
            Assert.IsTrue(result);
            Assert.AreEqual(1, lib.GetTotalTitles());
        }
        // TODO: nem létező cím eltávolításakor false-t kell visszaadni
        public void RemoveBook_NotExistingTitle()
        {
            var lib = CreateDefaultLibrary(); 
            bool result = lib.RemoveBook("asdd");
            Assert.IsFalse(result);
           

        }
        // TODO: eltávolítás után a cím már nem érhető el, GetAvailableCopies -1-et ad vissza
        public void RemoveBook_DeleteExistingTitle()
        {
            var lib = CreateDefaultLibrary();
            lib.RemoveBook("Dune");
            Assert.AreEqual(-1, lib.GetAvailableCopies("Dune"));


        }
    }
}

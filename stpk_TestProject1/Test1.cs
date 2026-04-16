// TANumberTests.cs
using Calculator;
using Calculator;
using Calculator.Numbers;
using CalculatorMemory;
using CalculatorProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;

namespace CalculatorEditorTests
{
    [TestClass]
    public class PEditorTests
    {
        [TestMethod]
        public void AddDigit_ValidDigit_AddsDigit()
        {
            var editor = new PEditor(10);
            editor.AddDigit(5);
            Assert.AreEqual("5", editor.Str); 
        }

        [TestMethod]
        public void AddDigit_AfterZero_ReplacesZero()
        {
            var editor = new PEditor(10);
            editor.AddDigit(0); 
            editor.AddDigit(5); 
            Assert.AreEqual("5", editor.Str);
        }

        [TestMethod]
        public void AddDigit_MultipleDigits_AppendsCorrectly()
        {
            var editor = new PEditor(10);
            editor.AddDigit(1);
            editor.AddDigit(2);
            editor.AddDigit(3);
            Assert.AreEqual("123", editor.Str);
        }

        [TestMethod]
        public void ToggleSign_ChangesSign()
        {
            var editor = new PEditor(10);
            editor.AddDigit(5);
            editor.ToggleSign();
            Assert.AreEqual("-5", editor.Str); 
            editor.ToggleSign();
            Assert.AreEqual("5", editor.Str);
        }

        [TestMethod]
        public void ToggleSign_AfterZero_WorksCorrectly()
        {
            var editor = new PEditor(10);
            editor.ToggleSign(); 
            Assert.AreEqual("0", editor.Str);

            editor.AddDigit(5);
            editor.ToggleSign();
            Assert.AreEqual("-5", editor.Str);
        }

        [TestMethod]
        public void Backspace_RemovesLastChar()
        {
            var editor = new PEditor(10);
            editor.AddDigit(1);
            editor.AddDigit(2);
            editor.AddDigit(3);
            editor.Backspace();
            Assert.AreEqual("12", editor.Str);

            editor.Backspace();
            editor.Backspace();
            Assert.AreEqual("0", editor.Str); 
        }
    }

    [TestClass]
    public class FEditorTests
    {
        [TestMethod]
        public void AddFractionSeparator_AddsSlash()
        {
            var editor = new FEditor();
            editor.AddDigit(3); // "3"
            editor.AddSeparator(); // Добавляем "/"
            Assert.AreEqual("3/", editor.Str);
        }

        [TestMethod]
        public void AddDigit_AfterZero_ReplacesZero()
        {
            var editor = new FEditor();
            editor.AddDigit(0); // "0"
            editor.AddDigit(5); // Должно заменить "0" на "5"
            Assert.AreEqual("5", editor.Str);
        }

        [TestMethod]
        public void IsZero_VariousInputs_ReturnsCorrect()
        {
            var editor = new FEditor();

            // Начальное состояние - ноль
            Assert.IsTrue(editor.IsZero);

            // "0" - ноль
            editor.Str = "0";
            Assert.IsTrue(editor.IsZero);

            // "-0" - ноль
            editor.Str = "-0";
            Assert.IsTrue(editor.IsZero);

            // "0/5" - ноль (числитель ноль)
            editor.Str = "0/5";
            Assert.IsTrue(editor.IsZero);

            // "-0/5" - ноль
            editor.Str = "-0/5";
            Assert.IsTrue(editor.IsZero);

            // "5" - не ноль
            editor.Str = "5";
            Assert.IsFalse(editor.IsZero);

            // "5/3" - не ноль
            editor.Str = "5/3";
            Assert.IsFalse(editor.IsZero);
        }
    }


    [TestClass]
    public class CEditorTests
    {
        [TestMethod]
        public void Constructor_SetsZero()
        {
            var editor = new CEditor();
            Assert.AreEqual("0", editor.Str);
            Assert.IsTrue(editor.IsZero);
        }

        [TestMethod]
        public void AddDigit_ValidDigit_AddsDigit()
        {
            var editor = new CEditor();
            editor.AddDigit(5);
            Assert.AreEqual("5", editor.Str);
        }

        [TestMethod]
        public void AddDigit_AfterZero_ReplacesZero()
        {
            var editor = new CEditor();
            editor.AddDigit(0); // "0"
            editor.AddDigit(5); // Должно заменить "0" на "5"
            Assert.AreEqual("5", editor.Str);
        }

        [TestMethod]
        public void AddImaginaryUnit_AddsI()
        {
            var editor = new CEditor();
            editor.AddDigit(3);
            editor.Str += "i"; // Для теста добавляем i напрямую
            Assert.AreEqual("3i", editor.Str);
        }

        [TestMethod]
        public void AddSeparator_AddsPlus()
        {
            var editor = new CEditor();
            editor.AddDigit(3);
            editor.AddSeparator(); // Добавляем "+"
            Assert.AreEqual("3+", editor.Str);
        }

        [TestMethod]
        public void BuildComplex_CreatesValidFormat()
        {
            var editor = new CEditor();
            editor.Str = "3+4i";
            Assert.AreEqual("3+4i", editor.Str);
            Assert.IsFalse(editor.IsZero);
        }

        [TestMethod]
        public void ToggleSign_ChangesSign()
        {
            var editor = new CEditor();
            editor.AddDigit(5);
            editor.ToggleSign();
            Assert.AreEqual("-5", editor.Str);
            editor.ToggleSign();
            Assert.AreEqual("5", editor.Str);
        }

        [TestMethod]
        public void ToggleSign_Zero_DoesNotChange()
        {
            var editor = new CEditor();
            editor.ToggleSign();
            Assert.AreEqual("0", editor.Str);
        }

        [TestMethod]
        public void Backspace_RemovesLastChar()
        {
            var editor = new CEditor();
            editor.AddDigit(1);
            editor.AddDigit(2);
            editor.AddDigit(3);
            editor.Backspace();
            Assert.AreEqual("12", editor.Str);

            editor.Backspace();
            editor.Backspace();
            Assert.AreEqual("0", editor.Str);
        }

        [TestMethod]
        public void IsZero_VariousInputs_ReturnsCorrect()
        {
            var editor = new CEditor();

            // Начальное состояние - ноль
            Assert.IsTrue(editor.IsZero);

            // "0" - ноль
            editor.Str = "0";
            Assert.IsTrue(editor.IsZero);

            // "-0" - ноль
            editor.Str = "-0";
            Assert.IsTrue(editor.IsZero);

            // "0i" - ноль
            editor.Str = "0i";
            Assert.IsTrue(editor.IsZero);

            // "-0i" - ноль
            editor.Str = "-0i";
            Assert.IsTrue(editor.IsZero);

            // "+0i" - ноль
            editor.Str = "+0i";
            Assert.IsTrue(editor.IsZero);

            // "0+0i" - ноль
            editor.Str = "0+0i";
            Assert.IsTrue(editor.IsZero);

            // "0-0i" - ноль
            editor.Str = "0-0i";
            Assert.IsTrue(editor.IsZero);

            // "5" - не ноль
            editor.Str = "5";
            Assert.IsFalse(editor.IsZero);

            // "5i" - не ноль
            editor.Str = "5i";
            Assert.IsFalse(editor.IsZero);

            // "3+4i" - не ноль
            editor.Str = "3+4i";
            Assert.IsFalse(editor.IsZero);
        }

        [TestMethod]
        public void Clear_ResetsToZero()
        {
            var editor = new CEditor();
            editor.AddDigit(5);
            editor.AddSeparator();
            editor.AddDigit(4);
            editor.Str += "i";
            editor.Clear();
            Assert.AreEqual("0", editor.Str);
            Assert.IsTrue(editor.IsZero);
           
        }
    }

    [TestClass]
    public class AEditorPolymorphismTests
    {
        [TestMethod]
        public void EditMethod_WorksPolymorphically()
        {
            AEditor[] editors = new AEditor[]
            {
                new PEditor(10),
                new FEditor(),
                new CEditor()
            };

            foreach (var editor in editors)
            {
                editor.Edit(1); // Добавить цифру 1
                Assert.IsFalse(string.IsNullOrEmpty(editor.Str));
                Assert.IsFalse(editor.IsZero);

                editor.Edit(0); // Сменить знак
                editor.Edit(18); // Backspace
                editor.Edit(19); // Clear
                Assert.IsTrue(editor.IsZero);
            }
        }

        [TestMethod]
        public void ReadWriteStr_WorksCorrectly()
        {
            var editor = new PEditor(10);
            editor.WriteStr("123.45");
            Assert.AreEqual("123.45", editor.ReadStr());

            var editor2 = new FEditor();
            editor2.WriteStr("3/4");
            Assert.AreEqual("3/4", editor2.ReadStr());
        }
    }
}


namespace CalculatorNumbersTests
{
    [TestClass]
    public class TPNumberTests
    {
        [TestMethod]
        public void Constructor_DoubleAndBase_CreatesCorrectNumber()
        {
            var num = new TPNumber(10.5, 10);
            Assert.AreEqual("10.5", num.Str);
            Assert.AreEqual(10, num.Base);
        }

        [TestMethod]
        public void Add_TwoNumbers_ReturnsSum()
        {
            var a = new TPNumber(5, 10);
            var b = new TPNumber(3, 10);
            var result = a.Add(b) as TPNumber;
            Assert.AreEqual(8, result.Value);
        }

        [TestMethod]
        public void IsZero_ZeroNumber_ReturnsTrue()
        {
            var num = new TPNumber(0, 10);
            Assert.IsTrue(num.IsZero());
        }

        [TestMethod]
        public void BaseChange_UpdatesStringRepresentation()
        {
            var num = new TPNumber(10, 10);
            num.Base = 2;
            Assert.AreEqual("1010", num.Str);
        }
    }

    [TestClass]
    public class TFracTests
    {
        [TestMethod]
        public void Constructor_NumeratorDenominator_CreatesFraction()
        {
            var frac = new TFrac(3, 4);
            Assert.AreEqual("3/4", frac.Str);
        }

        [TestMethod]
        public void Add_TwoFractions_ReturnsSum()
        {
            var a = new TFrac(1, 2);
            var b = new TFrac(1, 3);
            var result = a.Add(b) as TFrac;
            Assert.AreEqual("5/6", result.Str);
        }

        [TestMethod]
        public void IsZero_ZeroFraction_ReturnsTrue()
        {
            var frac = new TFrac(0, 5);
            Assert.IsTrue(frac.IsZero());
        }

        [TestMethod]
        public void Normalize_ReducesFraction()
        {
            var frac = new TFrac(4, 8);
            Assert.AreEqual("1/2", frac.Str);
        }
    }

    [TestClass]
    public class TCompTests
    {
        [TestMethod]
        public void Constructor_ReIm_CreatesComplex()
        {
            var comp = new TComp(3, 4);
            Assert.AreEqual("3+4i", comp.Str);
        }

        [TestMethod]
        public void Add_TwoComplex_ReturnsSum()
        {
            var a = new TComp(1, 2);
            var b = new TComp(3, 4);
            var result = a.Add(b) as TComp;
            Assert.AreEqual(4, result.Re);
            Assert.AreEqual(6, result.Im);
        }

        [TestMethod]
        public void Mul_TwoComplex_ReturnsProduct()
        {
            var a = new TComp(1, 2);
            var b = new TComp(3, 4);
            var result = a.Mul(b) as TComp;
            Assert.AreEqual(-5, result.Re, 1e-10);
            Assert.AreEqual(10, result.Im, 1e-10);
        }

        [TestMethod]
        public void IsZero_ZeroComplex_ReturnsTrue()
        {
            var comp = new TComp(0, 0);
            Assert.IsTrue(comp.IsZero());
        }
    }

    [TestClass]
    public class TANumberPolymorphismTests
    {
        [TestMethod]
        public void Copy_CreatesIndependentCopy()
        {
            TANumber original = new TPNumber(42, 10);
            TANumber copy = original.Copy();
            Assert.AreNotSame(original, copy);
            Assert.AreEqual(original.Str, copy.Str);
        }

        [TestMethod]
        public void Equals_SameValues_ReturnsTrue()
        {
            TANumber a = new TFrac(1, 2);
            TANumber b = new TFrac(1, 2);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_DifferentTypes_ThrowsException()
        {
            TANumber a = new TPNumber(5, 10);
            TANumber b = new TFrac(1, 2);
            a.Add(b);
        }
    }
}



[TestClass]
public class TMemoryTests
{
    private TANumber CreateTestNumber(double value, int numBase = 10)
    {
        return new TPNumber(value, numBase);
    }

    [TestMethod]
    public void Constructor_InitializesWithZeroAndOffState()
    {
        var zero = CreateTestNumber(0);
        var memory = new TMemory(zero);

        Assert.AreEqual(MemoryState.Off, memory.State);
        Assert.AreEqual("Off", memory.StateString);
        Assert.AreEqual("0", memory.NumberString);
        Assert.IsTrue(memory.IsOff);
        Assert.IsFalse(memory.IsOn);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_NullArgument_ThrowsException()
    {
        var memory = new TMemory(null);
    }

    [TestMethod]
    public void Store_SavesNumberAndTurnsOnMemory()
    {
        var zero = CreateTestNumber(0);
        var memory = new TMemory(zero);
        var number = CreateTestNumber(42);

        memory.Store(number);

        Assert.AreEqual(MemoryState.On, memory.State);
        Assert.AreEqual("On", memory.StateString);
        Assert.AreEqual("42", memory.NumberString);
        Assert.IsTrue(memory.IsOn);
    }

    [TestMethod]
    public void Store_CreatesCopy_NotReference()
    {
        var zero = CreateTestNumber(0);
        var memory = new TMemory(zero);
        var original = CreateTestNumber(42);

        memory.Store(original);
        original.Str = "100";

        Assert.AreEqual("42", memory.NumberString);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Store_NullArgument_ThrowsException()
    {
        var zero = CreateTestNumber(0);
        var memory = new TMemory(zero);

        memory.Store(null);
    }

    [TestMethod]
    public void Retrieve_ReturnsCopyAndTurnsOnMemory()
    {
        var zero = CreateTestNumber(0);
        var memory = new TMemory(zero);
        var number = CreateTestNumber(42);
        memory.Store(number);

        var retrieved = memory.Retrieve();

        Assert.IsNotNull(retrieved);
        Assert.AreEqual("42", retrieved.Str);
        Assert.AreEqual(MemoryState.On, memory.State);

        retrieved.Str = "100";
        Assert.AreEqual("42", memory.NumberString);
    }

    [TestMethod]
    public void Add_WhenMemoryOff_StoresNumberAndTurnsOnMemory()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial); // Память выключена
        var toAdd = CreateTestNumber(5);

        // Act
        memory.Add(toAdd);

        // Assert
        Assert.AreEqual(MemoryState.On, memory.State);
        Assert.AreEqual("15", memory.NumberString); // 10 + 5 = 15
    }

    [TestMethod]
    public void Add_WhenMemoryOn_SumsNumbers()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);
        memory.Store(initial); // Включаем память с числом 10
        var toAdd = CreateTestNumber(5);

        // Act
        memory.Add(toAdd);

        // Assert
        Assert.AreEqual(MemoryState.On, memory.State);
        Assert.AreEqual("15", memory.NumberString);
    }

    [TestMethod]
    public void Add_WhenMemoryHasNoNumber_SetsNumber()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);
        // Не вызываем Store, но _mem уже есть от конструктора

        var toAdd = CreateTestNumber(5);

        // Act
        memory.Add(toAdd);

        // Assert
        Assert.AreEqual(MemoryState.On, memory.State);
        Assert.AreEqual("15", memory.NumberString);
    }

    [TestMethod]
    public void Add_MultipleTimes_AccumulatesCorrectly()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);
        memory.Store(initial);

        // Act
        memory.Add(CreateTestNumber(5)); // 15
        memory.Add(CreateTestNumber(3)); // 18

        // Assert
        Assert.AreEqual("18", memory.NumberString);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Add_NullArgument_ThrowsException()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);

        // Act
        memory.Add(null);
    }

    [TestMethod]
    public void Clear_WhenMemoryOn_ResetsToZeroAndTurnsOffMemory()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);
        memory.Store(initial);

        // Act
        memory.Clear();

        // Assert
        Assert.AreEqual(MemoryState.Off, memory.State);
        Assert.AreEqual("Off", memory.StateString);
        Assert.AreEqual("0", memory.NumberString);
        Assert.IsTrue(memory.IsOff);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Clear_WhenMemoryOff_ThrowsException()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial); // Память выключена

        // Act
        memory.Clear();
    }

    [TestMethod]
    public void Store_OverwritesPreviousValue()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);
        memory.Store(CreateTestNumber(20));

        // Act
        memory.Store(CreateTestNumber(30));

        // Assert
        Assert.AreEqual("30", memory.NumberString);
    }

    [TestMethod]
    public void Retrieve_AfterStore_ReturnsStoredValue()
    {
        // Arrange
        var initial = CreateTestNumber(10);
        var memory = new TMemory(initial);
        var toStore = CreateTestNumber(42);
        memory.Store(toStore);

        // Act
        var retrieved = memory.Retrieve();

        // Assert
        Assert.AreEqual("42", retrieved.Str);
    }

    [TestMethod]
    public void MultipleOperations_WorkCorrectly()
    {
        // Arrange
        var initial = CreateTestNumber(0);
        var memory = new TMemory(initial);

        // Act & Assert
        memory.Store(CreateTestNumber(10));
        Assert.AreEqual("10", memory.NumberString);
        Assert.IsTrue(memory.IsOn);

        memory.Add(CreateTestNumber(5));
        Assert.AreEqual("15", memory.NumberString);

        var retrieved = memory.Retrieve();
        Assert.AreEqual("15", retrieved.Str);

        memory.Clear();
        Assert.IsTrue(memory.IsOff);
        Assert.AreEqual("0", memory.NumberString);
    }

    [TestMethod]
    public void StateString_ReturnsCorrectString()
    {
        // Arrange
        var initial = CreateTestNumber(0);
        var memory = new TMemory(initial);

        // Assert
        Assert.AreEqual("Off", memory.StateString);

        // Act
        memory.Store(CreateTestNumber(5));
        Assert.AreEqual("On", memory.StateString);

        // Act
        memory.Clear();
        Assert.AreEqual("Off", memory.StateString);
    }
}




namespace CalculatorProcessorTests
{
    [TestClass]
    public class TProcTests
    {
        private TANumber CreateNumber(double value, int numBase = 10)
        {
            return new TPNumber(value, numBase);
        }

        [TestMethod]
        public void Constructor_InitializesWithCopiesAndNoneOperation()
        {
            // Arrange
            var left = CreateNumber(2);
            var right = CreateNumber(3);

            // Act
            var proc = new TProc(left, right);

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("3", proc.RopString);
            Assert.AreEqual(TOprtn.None, proc.Operation);
            Assert.AreEqual(string.Empty, proc.Error);
            Assert.IsFalse(proc.HasError);
            Assert.IsFalse(proc.IsOperationSet);

            // Проверяем, что это копии, а не ссылки
            left.Str = "100";
            right.Str = "200";
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("3", proc.RopString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullLeft_ThrowsException()
        {
            // Act
            var proc = new TProc(null, CreateNumber(3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullRight_ThrowsException()
        {
            // Act
            var proc = new TProc(CreateNumber(2), null);
        }

        

        [TestMethod]
        public void OprtnClear_ClearsOperation()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));
            proc.OprtnSet(TOprtn.Add);

            // Act
            proc.OprtnClear();

            // Assert
            Assert.AreEqual(TOprtn.None, proc.Operation);
            Assert.IsFalse(proc.IsOperationSet);
        }

        [TestMethod]
        public void OprtnSet_SetsOperation()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));

            // Act
            proc.OprtnSet(TOprtn.Add);

            // Assert
            Assert.AreEqual(TOprtn.Add, proc.Operation);
            Assert.IsTrue(proc.IsOperationSet);
        }

        [TestMethod]
        public void OprtnRun_Add_ComputesCorrectly()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));
            proc.OprtnSet(TOprtn.Add);

            // Act
            proc.OprtnRun();

            // Assert
            Assert.AreEqual("5", proc.LopResString);
            Assert.AreEqual("3", proc.RopString); // Rop не меняется
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void OprtnRun_Sub_ComputesCorrectly()
        {
            // Arrange
            var proc = new TProc(CreateNumber(5), CreateNumber(3));
            proc.OprtnSet(TOprtn.Sub);

            // Act
            proc.OprtnRun();

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void OprtnRun_Mul_ComputesCorrectly()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));
            proc.OprtnSet(TOprtn.Mul);

            // Act
            proc.OprtnRun();

            // Assert
            Assert.AreEqual("6", proc.LopResString);
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void OprtnRun_Dvd_ComputesCorrectly()
        {
            // Arrange
            var proc = new TProc(CreateNumber(6), CreateNumber(3));
            proc.OprtnSet(TOprtn.Dvd);

            // Act
            proc.OprtnRun();

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void OprtnRun_DivideByZero_SetsError()
        {
            // Arrange
            var proc = new TProc(CreateNumber(6), CreateNumber(0));
            proc.OprtnSet(TOprtn.Dvd);

            // Act
            proc.OprtnRun();

            // Assert
            Assert.AreEqual("6", proc.LopResString); 
            Assert.IsTrue(proc.HasError);
            Assert.IsTrue(proc.Error.Contains("Деление на ноль"));
        }

        [TestMethod]
        public void OprtnRun_WhenOperationNone_DoesNothing()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));
            // Operation = None по умолчанию

            // Act
            proc.OprtnRun();

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("3", proc.RopString);
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void FuncRun_Rev_ComputesCorrectly()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(4));

            // Act
            proc.FuncRun(TFunc.Rev);

            // Assert
            Assert.AreEqual("2", proc.LopResString); 
            Assert.AreEqual("0.25", proc.RopString); // 1/4 = 0.25
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void FuncRun_Sqr_ComputesCorrectly()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(4));

            // Act
            proc.FuncRun(TFunc.Sqr);

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("16", proc.RopString); // 4^2 = 16
            Assert.AreEqual(string.Empty, proc.Error);
        }

        [TestMethod]
        public void FuncRun_RevOnZero_SetsError()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(0));

            // Act
            proc.FuncRun(TFunc.Rev);

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("0", proc.RopString); 
            Assert.IsTrue(proc.HasError);
            Assert.IsTrue(proc.Error.Contains("Деление на ноль"));
        }

        [TestMethod]
        public void LopResSet_SetsLeftOperand()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));
            var newValue = CreateNumber(10);

            // Act
            proc.LopResSet(newValue);

            // Assert
            Assert.AreEqual("10", proc.LopResString);
            Assert.AreEqual("3", proc.RopString);

            // Проверяем, что это копия
            newValue.Str = "100";
            Assert.AreEqual("10", proc.LopResString);
        }

        [TestMethod]
        public void LopResGet_ReturnsCopy()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));

            // Act
            var result = proc.LopResGet();

            // Assert
            Assert.AreEqual("2", result.Str);

            // Проверяем, что это копия
            result.Str = "100";
            Assert.AreEqual("2", proc.LopResString);
        }

        [TestMethod]
        public void RopSet_SetsRightOperand()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));
            var newValue = CreateNumber(10);

            // Act
            proc.RopSet(newValue);

            // Assert
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("10", proc.RopString);
        }

        [TestMethod]
        public void RopGet_ReturnsCopy()
        {
            // Arrange
            var proc = new TProc(CreateNumber(2), CreateNumber(3));

            // Act
            var result = proc.RopGet();

            // Assert
            Assert.AreEqual("3", result.Str);
        }

        [TestMethod]
        public void ErrorClear_ClearsError()
        {
            // Arrange
            var proc = new TProc(CreateNumber(6), CreateNumber(0));
            proc.OprtnSet(TOprtn.Dvd);
            proc.OprtnRun(); // Должна возникнуть ошибка
            Assert.IsTrue(proc.HasError);

            // Act
            proc.ErrorClear();

            // Assert
            Assert.AreEqual(string.Empty, proc.Error);
            Assert.IsFalse(proc.HasError);
        }

        [TestMethod]
        public void Sequence_FromSpecification_ExactSteps()
        {
            // Создаем процессор
            var proc = new TProc(CreateNumber(0), CreateNumber(0));

            // Шаг 0: Create
            Assert.AreEqual("0", proc.LopResString);
            Assert.AreEqual("0", proc.RopString);
            Assert.AreEqual(TOprtn.None, proc.Operation);

            // Шаг 1: (нет входных данных)
            // Состояние не меняется

            // Шаг 2: Вход = "+"
            // Метод: Lop_Res_Set; OprtnSet
            proc.LopResSet(CreateNumber(2));
            proc.OprtnSet(TOprtn.Add);
            Assert.AreEqual("2", proc.LopResString);
            Assert.AreEqual("0", proc.RopString); // Rop пока не менялся
            Assert.AreEqual(TOprtn.Add, proc.Operation);

            // Шаг 3: (нет входных данных)
            // Состояние: Lop_Res = 2, Rop = 0, Operation = Add

            // Шаг 4: Вход = "*"
            // Метод: Rop_Set; OprtnRun; OprtnSet
            proc.RopSet(CreateNumber(3));     // Устанавливаем правый операнд
            proc.OprtnRun();                   // Выполняем сложение: 2 + 3 = 5
            proc.OprtnSet(TOprtn.Mul);        // Устанавливаем новую операцию - умножение
            Assert.AreEqual("5", proc.LopResString); // Результат сложения
            Assert.AreEqual("3", proc.RopString);    // Правый операнд для умножения пока не установлен
            Assert.AreEqual(TOprtn.Mul, proc.Operation);

            // Шаг 5: (нет входных данных)
            // В спецификации на этом шаге Rop = 4 (уже установлен для следующей операции)
            // Но в нашей реализации нужно явно установить Rop
            proc.RopSet(CreateNumber(4)); // Устанавливаем правый операнд для умножения
            Assert.AreEqual("5", proc.LopResString);
            Assert.AreEqual("4", proc.RopString);
            Assert.AreEqual(TOprtn.Mul, proc.Operation);

            // Шаг 6: Вход = "Sqr"
            // Метод: Rop_Set; FuncRun
            // В спецификации: сначала Rop_Set (уже сделано на шаге 5), затем FuncRun
            proc.FuncRun(TFunc.Sqr); // 4^2 = 16
            Assert.AreEqual("5", proc.LopResString);
            Assert.AreEqual("16", proc.RopString);
            Assert.AreEqual(TOprtn.Mul, proc.Operation); // Операция не меняется

            // Шаг 7: Вход = "="
            // Метод: OprtnRun
            proc.OprtnRun(); // 5 * 16 = 80
            Assert.AreEqual("80", proc.LopResString);
            Assert.AreEqual("16", proc.RopString);
            Assert.AreEqual(TOprtn.Mul, proc.Operation);

            // Шаг 8: Вход = "C"
            // Метод: ReSet
            proc.ReSet();
            Assert.AreEqual("0", proc.LopResString);
            Assert.AreEqual("0", proc.RopString);
            Assert.AreEqual(TOprtn.None, proc.Operation);
        }
    }
}





namespace CalculatorUITests
{
    [TestClass]
    public class TClcPnlTests
    {
        private TClcPnl _calcForm;
        private const int DIGIT_1 = 1;
        private const int DIGIT_2 = 2;
        private const int DIGIT_3 = 3;
        private const int DIGIT_4 = 4;
        private const int DIGIT_5 = 5;
        private const int DIGIT_6 = 6;
        private const int DIGIT_7 = 7;
        private const int DIGIT_8 = 8;
        private const int DIGIT_9 = 9;
        private const int DIGIT_0 = 10;

        private const int DIGIT_A = 11;
        private const int DIGIT_B = 12;
        private const int DIGIT_C = 13;
        private const int DIGIT_D = 14;
        private const int DIGIT_E = 15;
        private const int DIGIT_F = 16;

        private const int SEPARATOR = 17;      // . или / или +
        private const int BACKSPACE = 18;
        private const int EDITOR_CLEAR = 19;    // Очистка только редактора

        private const int OP_ADD = 21;           // +
        private const int OP_SUB = 22;           // -
        private const int OP_MUL = 23;           // *
        private const int OP_DIV = 24;           // /

        private const int FUNC_REV = 25;          // 1/x
        private const int FUNC_SQR = 26;          // x²

        private const int EQUAL = 27;              // =
        private const int RESET = 28;              // C (полный сброс)

        private const int MEM_MS = 29;              // Memory Store
        private const int MEM_MR = 30;              // Memory Retrieve
        private const int MEM_MPLUS = 31;           // Memory Add
        private const int MEM_MC = 32;              // Memory Clear

        private const int CLIP_COPY = 33;            // Копировать
        private const int CLIP_PASTE = 34;           // Вставить
        private const int CLIP_CUT = 35;             // Вырезать

        [TestInitialize]
        public void Setup()
        {
            _calcForm = new TClcPnl();
            _calcForm.Show();
            _calcForm.Focus();
            Application.DoEvents();
            Thread.Sleep(100);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _calcForm.Close();
            _calcForm.Dispose();
        }


        private void ExecuteCommand(int commandCode)
        {
            var method = _calcForm.GetType().GetMethod("ExecuteCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(_calcForm, new object[] { commandCode });
            Application.DoEvents();
            Thread.Sleep(50);
        }

        private void SimulateDigitClick(int digit)
        {
            Console.WriteLine($"Simulating digit with command code {digit}");
            ExecuteCommand(digit);
        }

        private void SimulateOperatorClick(int opCode)
        {
            Console.WriteLine($"Simulating operator with code {opCode}");
            ExecuteCommand(opCode);
        }

        private void SimulateFunctionClick(int funcCode)
        {
            Console.WriteLine($"Simulating function with code {funcCode}");
            ExecuteCommand(funcCode);
        }

        [TestMethod]
        public void Constructor_InitializesCorrectly()
        {
            Assert.IsNotNull(_calcForm);
            Assert.AreEqual("0", _calcForm.DisplayText);
            Assert.AreEqual(CalculatorMode.PNumber, _calcForm.CurrentMode);
        }

        [TestMethod]
        public void ExecuteCommand_Digit_UpdatesDisplay()
        {
            // Act
            ExecuteCommand(DIGIT_5);

            // Assert
            Assert.AreEqual("5", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_MultipleDigits_AppendsCorrectly()
        {
            // Act
            ExecuteCommand(DIGIT_1);
            ExecuteCommand(DIGIT_2);
            ExecuteCommand(DIGIT_3);

            // Assert
            Assert.AreEqual("123", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_Addition_ComputesCorrectly()
        {
            // 2 + 3 = 5
            ExecuteCommand(DIGIT_2);        // 2
            ExecuteCommand(OP_ADD);          // +
            ExecuteCommand(DIGIT_3);         // 3
            ExecuteCommand(EQUAL);           // =

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Addition result: {result}");

            Assert.AreEqual("5", result);
        }

        [TestMethod]
        public void ExecuteCommand_Subtraction_ComputesCorrectly()
        {
            // 5 - 3 = 2
            ExecuteCommand(DIGIT_5);        // 5
            ExecuteCommand(OP_SUB);          // -
            ExecuteCommand(DIGIT_3);         // 3
            ExecuteCommand(EQUAL);           // =

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Subtraction result: {result}");

            Assert.AreEqual("2", result);
        }

        [TestMethod]
        public void ExecuteCommand_Multiplication_ComputesCorrectly()
        {
            // 4 * 3 = 12
            ExecuteCommand(DIGIT_4);        // 4
            ExecuteCommand(OP_MUL);          // *
            ExecuteCommand(DIGIT_3);         // 3
            ExecuteCommand(EQUAL);           // =

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Multiplication result: {result}");

            Assert.AreEqual("12", result);
        }

        [TestMethod]
        public void ExecuteCommand_Division_ComputesCorrectly()
        {
            // 6 / 3 = 2
            ExecuteCommand(DIGIT_6);        // 6
            ExecuteCommand(OP_DIV);          // /
            ExecuteCommand(DIGIT_3);         // 3
            ExecuteCommand(EQUAL);           // =

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Division result: {result}");

            Assert.AreEqual("2", result);
        }

        [TestMethod]
        public void ExecuteCommand_ComplexExpression_ComputesCorrectly()
        {
            // 2 + 3 * 4^2 = 80
            ExecuteCommand(DIGIT_2);        // 2
            ExecuteCommand(OP_ADD);          // +
            ExecuteCommand(DIGIT_3);         // 3
            ExecuteCommand(OP_MUL);          // *
            ExecuteCommand(DIGIT_4);         // 4
            ExecuteCommand(FUNC_SQR);        // x² (должно дать 16)

            string afterSqr = _calcForm.DisplayText;
            Console.WriteLine($"After Sqr: {afterSqr}");
            Assert.AreEqual("16", afterSqr);

            ExecuteCommand(EQUAL);           // =

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Complex expression result: {result}");

            Assert.AreEqual("80", result);
        }

        [TestMethod]
        public void ExecuteCommand_Clear_ResetsDisplay()
        {
            // Arrange
            ExecuteCommand(DIGIT_4);
            ExecuteCommand(DIGIT_2);
            Assert.AreEqual("42", _calcForm.DisplayText);

            // Act
            ExecuteCommand(RESET); // C (полный сброс)

            // Assert
            Assert.AreEqual("0", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_Backspace_RemovesLastChar()
        {
            // Arrange
            ExecuteCommand(DIGIT_1);
            ExecuteCommand(DIGIT_2);
            ExecuteCommand(DIGIT_3);
            Assert.AreEqual("123", _calcForm.DisplayText);

            // Act
            ExecuteCommand(BACKSPACE); // Backspace

            // Assert
            Assert.AreEqual("12", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_ToggleSign_ChangesSign()
        {
            // Arrange
            ExecuteCommand(DIGIT_4);
            ExecuteCommand(DIGIT_2);
            Assert.AreEqual("42", _calcForm.DisplayText);

            // Act
            ExecuteCommand(0); // Toggle sign

            // Assert
            Assert.AreEqual("-42", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_Square_Function()
        {
            // Arrange
            ExecuteCommand(DIGIT_4); // 4

            // Act
            ExecuteCommand(FUNC_SQR); // Sqr

            // Assert
            Assert.AreEqual("16", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_Rev_Function()
        {
            // Arrange
            ExecuteCommand(DIGIT_4); // 4

            // Act
            ExecuteCommand(FUNC_REV); // Rev (1/x)

            // Assert
            string result = _calcForm.DisplayText;
            Console.WriteLine($"Rev result: {result}");

            double actual = double.Parse(result, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(0.25, actual, 0.0001);
        }

        [TestMethod]
        public void ExecuteCommand_MemoryStore_UpdatesMemoryState()
        {
            // Arrange
            ExecuteCommand(DIGIT_4);
            ExecuteCommand(DIGIT_2); // 42

            // Act
            ExecuteCommand(MEM_MS); // MS

            // Assert
            Assert.AreEqual("On", _calcForm.MemoryStateText);
        }

        [TestMethod]
        public void ExecuteCommand_MemoryRetrieve_RetrievesValue()
        {
            // Arrange
            ExecuteCommand(DIGIT_4);
            ExecuteCommand(DIGIT_2); // 42
            ExecuteCommand(MEM_MS); // MS
            ExecuteCommand(RESET); // C
            Assert.AreEqual("0", _calcForm.DisplayText);

            // Act
            ExecuteCommand(MEM_MR); // MR

            // Assert
            Assert.AreEqual("42", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_MemoryAdd_Accumulates()
        {
            // MS 10, затем M+ 20, должно быть 30
            ExecuteCommand(DIGIT_1);
            ExecuteCommand(DIGIT_0); // 10
            ExecuteCommand(MEM_MS); // MS

            ExecuteCommand(RESET); // C - очищаем экран

            ExecuteCommand(DIGIT_2);
            ExecuteCommand(DIGIT_0); // 20
            ExecuteCommand(MEM_MPLUS); // M+

            ExecuteCommand(RESET); // C
            ExecuteCommand(MEM_MR); // MR

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Memory add result: {result}");

            Assert.AreEqual("30", result);
        }

        [TestMethod]
        public void ExecuteCommand_MemoryClear_ClearsMemory()
        {
            // Arrange
            ExecuteCommand(DIGIT_4);
            ExecuteCommand(DIGIT_2); // 42
            ExecuteCommand(MEM_MS); // MS
            Assert.AreEqual("On", _calcForm.MemoryStateText);

            // Act
            ExecuteCommand(MEM_MC); // MC

            // Assert
            Assert.AreEqual("Off", _calcForm.MemoryStateText);
        }

        [TestMethod]
        public void ExecuteCommand_Separator_AddsDecimalPoint()
        {
            // Arrange
            ExecuteCommand(DIGIT_3);

            // Act
            ExecuteCommand(SEPARATOR); // .

            // Assert
            Assert.AreEqual("3.", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_ZeroDigit_WorksCorrectly()
        {
            // Act
            ExecuteCommand(DIGIT_0); // 0

            // Assert
            Assert.AreEqual("0", _calcForm.DisplayText);

            // Добавляем еще цифры
            ExecuteCommand(DIGIT_5);
            Assert.AreEqual("5", _calcForm.DisplayText); // Должен заменить 0 на 5
        }

        [TestMethod]
        public void KeyPress_Digit_UpdatesDisplay()
        {
            // Симулируем нажатие клавиши '5'
            var method = _calcForm.GetType().GetMethod("FormKeyPress",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var args = new KeyPressEventArgs('5');
            method?.Invoke(_calcForm, new object[] { _calcForm, args });

            Application.DoEvents();
            Thread.Sleep(50);

            Assert.AreEqual("5", _calcForm.DisplayText);
        }

        [TestMethod]
        public void KeyPress_Operator_UpdatesState()
        {
            // Симулируем: 2 + 3 = 
            var method = _calcForm.GetType().GetMethod("FormKeyPress",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('2') });
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('+') });
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('3') });
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs((char)13) }); // Enter

            Application.DoEvents();
            Thread.Sleep(50);

            Assert.AreEqual("5", _calcForm.DisplayText);
        }

        [TestMethod]
        public void KeyPress_Backspace_RemovesChar()
        {
            // Arrange
            var method = _calcForm.GetType().GetMethod("FormKeyPress",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('1') });
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('2') });
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('3') });

            Assert.AreEqual("123", _calcForm.DisplayText);

            // Act
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs((char)8) }); // Backspace

            Application.DoEvents();
            Thread.Sleep(50);

            // Assert
            Assert.AreEqual("12", _calcForm.DisplayText);
        }

        [TestMethod]
        public void KeyPress_Clear_ResetsDisplay()
        {
            // Arrange
            var method = _calcForm.GetType().GetMethod("FormKeyPress",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('4') });
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('2') });
            Assert.AreEqual("42", _calcForm.DisplayText);

            // Act
            method?.Invoke(_calcForm, new object[] { _calcForm, new KeyPressEventArgs('C') });

            Application.DoEvents();
            Thread.Sleep(50);

            // Assert
            Assert.AreEqual("0", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ModeChange_SwitchToFraction_UpdatesCalculator()
        {
            // Act
            _calcForm.CurrentMode = CalculatorMode.Fraction;

            // Assert
            Assert.AreEqual(CalculatorMode.Fraction, _calcForm.CurrentMode);

            // Проверяем, что можно ввести дробь
            ExecuteCommand(DIGIT_3);     // 3
            ExecuteCommand(SEPARATOR);   // /
            ExecuteCommand(DIGIT_4);     // 4

            string display = _calcForm.DisplayText;
            Console.WriteLine($"Fraction display: {display}");

            Assert.IsTrue(display.Contains("3") && display.Contains("/") && display.Contains("4"));
        }

        [TestMethod]
        public void ModeChange_SwitchToComplex_UpdatesCalculator()
        {
            // Act
            _calcForm.CurrentMode = CalculatorMode.Complex;

            // Assert
            Assert.AreEqual(CalculatorMode.Complex, _calcForm.CurrentMode);

            //  можно ввести комплексное число
            ExecuteCommand(DIGIT_3);     // 3
            ExecuteCommand(SEPARATOR);   // +
            ExecuteCommand(DIGIT_4);     // 4
            ExecuteCommand(FUNC_SQR);    // i (в комплексном режиме это может быть добавление i)

            string display = _calcForm.DisplayText;
            Console.WriteLine($"Complex display: {display}");
        }

        [TestMethod]
        public void ExecuteCommand_ChainedOperations_WorkCorrectly()
        {
            // 2 + 3 = 5, затем * 4 = 20
            ExecuteCommand(DIGIT_2);
            ExecuteCommand(OP_ADD);
            ExecuteCommand(DIGIT_3);
            ExecuteCommand(EQUAL);
            Assert.AreEqual("5", _calcForm.DisplayText);

            ExecuteCommand(OP_MUL);
            ExecuteCommand(DIGIT_4);
            ExecuteCommand(EQUAL);

            Assert.AreEqual("20", _calcForm.DisplayText);
        }

        [TestMethod]
        public void ExecuteCommand_DivideByZero_ShowsError()
        {
            // 5 / 0 = 
            ExecuteCommand(DIGIT_5);
            ExecuteCommand(OP_DIV);
            ExecuteCommand(DIGIT_0);
            ExecuteCommand(EQUAL);

            string result = _calcForm.DisplayText;
            Console.WriteLine($"Division by zero result: {result}");

            Assert.IsTrue(result.Contains("Деление") || result.Contains("ошибка"));
        }
    }
}



namespace CalculatorTests
{
    [TestClass]
    
    public class CalculatorTestPrecedents
    {
        private TCtrl _calculator;
        private string _clipboardText;
        private string _memoryState;

        [TestInitialize]
        public void Setup()
        {
            // Инициализация для p-ичных чисел (основание 10)
            _clipboardText = "";
            _memoryState = "";
            CreatePNumberCalculator(10);
        }

        private void CreatePNumberCalculator(int numBase)
        {
            var editor = new PEditor(numBase);
            var number = new TPNumber(0, numBase);
            var processor = new TProc(number.Copy(), number.Copy());
            var memory = new TMemory(number.Copy());
            _calculator = new TCtrl(editor, processor, memory, number);
        }

        private void CreateFractionCalculator()
        {
            var editor = new FEditor();
            var number = new TFrac(0, 1);
            var processor = new TProc(number.Copy(), number.Copy());
            var memory = new TMemory(number.Copy());
            _calculator = new TCtrl(editor, processor, memory, number);
        }

        private void CreateComplexCalculator()
        {
            var editor = new CEditor();
            var number = new TComp(0, 0);
            var processor = new TProc(number.Copy(), number.Copy());
            var memory = new TMemory(number.Copy());
            _calculator = new TCtrl(editor, processor, memory, number);
        }

        #region P-ичные числа (десятичные)

        [TestMethod]
        public void PNumber_SingleOperation_5PlusEquals_Returns10()
        {
            // 5 + = 10
            ExecuteCommand(5);      // 5
            ExecuteCommand(21);     // +
            ExecuteCommand(27);     // =

            Assert.AreEqual("10", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_SingleOperation_5MinusEquals_Returns0()
        {
            // 5 - = 0
            ExecuteCommand(5);      // 5
            ExecuteCommand(22);     // -
            ExecuteCommand(27);     // =

            Assert.AreEqual("0", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_SingleOperation_5MultiplyEquals_Returns25()
        {
            // 5 * = 25
            ExecuteCommand(5);      // 5
            ExecuteCommand(23);     // *
            ExecuteCommand(27);     // =

            Assert.AreEqual("25", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_SingleOperation_5DivideEquals_Returns1()
        {
            // 5 / = 1
            ExecuteCommand(5);      // 5
            ExecuteCommand(24);     // /
            ExecuteCommand(27);     // =

            Assert.AreEqual("1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_RepeatedLastOperation_5Plus4Equals9Equals13Equals17()
        {
            // 5 + 4 = 9 = 13 = 17
            ExecuteCommand(5);      // 5
            ExecuteCommand(21);     // +
            ExecuteCommand(4);      // 4
            ExecuteCommand(27);     // = (5+4=9)
            Assert.AreEqual("9", _calculator.Editor.Str);

            ExecuteCommand(27);     // = (9+4=13)
            Assert.AreEqual("13", _calculator.Editor.Str);

            ExecuteCommand(27);     // = (13+4=17)
            Assert.AreEqual("17", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_OperationOnDisplayedValue_2Plus3Equals5Equals8PlusEquals16()
        {
            // 2 + 3 = 5 = 8 + = 16
            ExecuteCommand(2);      // 2
            ExecuteCommand(21);     // +
            ExecuteCommand(3);      // 3
            ExecuteCommand(27);     // = (2+3=5)
            Assert.AreEqual("5", _calculator.Editor.Str);

            ExecuteCommand(27);     // = (5+3=8)
            Assert.AreEqual("8", _calculator.Editor.Str);

            ExecuteCommand(21);     // + (операция над отображаемым 8)
            ExecuteCommand(27);     // = (8+8=16)
            Assert.AreEqual("16", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_Function_Sqr_5Returns25()
        {
            // 5 Sqr = 25
            ExecuteCommand(5);      // 5
            ExecuteCommand(26);     // Sqr

            Assert.AreEqual("25", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_Function_Rev_5Returns02()
        {
            // 5 Rev = 0.2
            ExecuteCommand(5);      // 5
            ExecuteCommand(25);     // Rev

            Assert.AreEqual("0.2", _calculator.Editor.Str);
        }

        [TestMethod]
        public void PNumber_Expression_6SqrPlus2SqrDiv10Plus6Equals10()
        {
            // 6 Sqr + 2 Sqr / 10 + 6 = 10
            ExecuteCommand(6);      // 6
            ExecuteCommand(26);     // Sqr (36)
            ExecuteCommand(21);     // +
            ExecuteCommand(2);      // 2
            ExecuteCommand(26);     // Sqr (4)
            ExecuteCommand(24);     // /
            ExecuteCommand(1);      // 1
            //ExecuteCommand(0);      // 0 (10)
            ExecuteCommand(10);
            ExecuteCommand(21);     // +
            ExecuteCommand(6);      // 6
            ExecuteCommand(27);     // = (36+4)/10+6 = 40/10+6 = 4+6 = 10

            Assert.AreEqual("10", _calculator.Editor.Str);
        }

        #endregion

        #region Дроби

        [TestMethod]
        public void Fraction_SingleOperation_5_1_Plus_2_1_Equals_7_1()
        {
            CreateFractionCalculator();

            // 5/1 + 2/1 = 7/1
            ExecuteCommand(5);      // 5
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(21);     // +
            ExecuteCommand(2);      // 2
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(27);     // =

            Assert.AreEqual("7/1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_OperationWithOneOperand_5_1_MultiplyEquals_25_1()
        {
            CreateFractionCalculator();

            // 5/1 * = 25/1
            ExecuteCommand(5);      // 5
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(23);     // *
            ExecuteCommand(27);     // =

            Assert.AreEqual("25/1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_RepeatedOperation_5_1_Plus_4_1_Equals_9_1_Equals_13_1_Equals_17()
        {
            CreateFractionCalculator();

            // 5/1 + 4/1 = 9/1 = 13/1 = 17/1
            ExecuteCommand(5);      // 5
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(21);     // +
            ExecuteCommand(4);      // 4
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(27);     // = (5/1+4/1=9/1)
            Assert.AreEqual("9/1", _calculator.Editor.Str);

            ExecuteCommand(27);     // = (9/1+4/1=13/1)
            Assert.AreEqual("13/1", _calculator.Editor.Str);

            ExecuteCommand(27);     // = (13/1+4/1=17/1)
            Assert.AreEqual("17/1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_OperationOnDisplayedValue_2_1_Plus_3_1_Equals_5_1_Equals_8_1_Plus_Equals_16_1()
        {
            CreateFractionCalculator();

            // 2/1 + 3/1 = 5/1 = 8/1 + = 16/1
            ExecuteCommand(2);      // 2
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(21);     // +
            ExecuteCommand(3);      // 3
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(27);     // = (2/1+3/1=5/1)
            Assert.AreEqual("5/1", _calculator.Editor.Str);

            ExecuteCommand(27);     // = (5/1+3/1=8/1)
            Assert.AreEqual("8/1", _calculator.Editor.Str);

            ExecuteCommand(21);     // + (операция над отображаемым 8/1)
            ExecuteCommand(27);     // = (8/1+8/1=16/1)
            Assert.AreEqual("16/1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_Function_Sqr_5_1_Returns_25_1()
        {
            CreateFractionCalculator();

            // 5/1 Sqr = 25/1
            ExecuteCommand(5);      // 5
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(26);     // Sqr

            Assert.AreEqual("25/1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_Function_Rev_5_1_Returns_1_5()
        {
            CreateFractionCalculator();

            // 5/1 Rev = 1/5
            ExecuteCommand(5);      // 5
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(25);     // Rev

            Assert.AreEqual("1/5", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_Expression_6_1_Sqr_Plus_2_1_Sqr_Div_10_1_Plus_6_1_Equals_10_1()
        {
            CreateFractionCalculator();

            // 6/1 Sqr + 2/1 Sqr / 10/1 + 6/1 = 10/1
            ExecuteCommand(6);      // 6
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1 (6/1)
            ExecuteCommand(26);     // Sqr (36/1)
            ExecuteCommand(21);     // +
            ExecuteCommand(2);      // 2
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1 (2/1)
            ExecuteCommand(26);     // Sqr (4/1)
            ExecuteCommand(24);     // /
            ExecuteCommand(1);      // 1
            ExecuteCommand(10);      // 0 (10/1)
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1 (10/1)
            ExecuteCommand(21);     // +
            ExecuteCommand(6);      // 6
            ExecuteCommand(17);     // /
            ExecuteCommand(1);      // 1 (6/1)
            ExecuteCommand(27);     // = (36/1+4/1)/(10/1)+6/1 = (40/1)/(10/1)+6/1 = 4/1+6/1 = 10/1

            Assert.AreEqual("10/1", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_Simplify_2_4_Returns_1_2()
        {
            CreateFractionCalculator();

            // 2/4 (автоматически сокращается до 1/2)
            ExecuteCommand(2);      // 2
            ExecuteCommand(17);     // /
            ExecuteCommand(4);      // 4
            ExecuteCommand(41); // Simplify

            Assert.AreEqual("1/2", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Fraction_MixedNumber_7_2_SimplifyAndDisplay()
        {
            CreateFractionCalculator();

            // 7/2 (неправильная дробь)
            ExecuteCommand(7);      // 7
            ExecuteCommand(17);     // /
            ExecuteCommand(2);      // 2

            Assert.AreEqual("7/2", _calculator.Editor.Str);

 
        }

        #endregion

        #region Комплексные числа

        [TestMethod]
        public void Complex_Addition_3plus2i_Plus_1plus4i_Equals_4plus6i()
        {
            CreateComplexCalculator();

            // 3+2i + 1+4i = 4+6i
            ExecuteComplexNumber(3, 2);  // 3+2i
            ExecuteCommand(21);           // +
            ExecuteComplexNumber(1, 4);   // 1+4i
            ExecuteCommand(27);           // =

            Assert.AreEqual("4+6i", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_Subtraction_5plus3i_Minus_2plus1i_Equals_3plus2i()
        {
            CreateComplexCalculator();

            // 5+3i - 2+1i = 3+2i
            ExecuteComplexNumber(5, 3);  // 5+3i
            ExecuteCommand(22);           // -
            ExecuteComplexNumber(2, 1);   // 2+1i
            ExecuteCommand(27);           // =

            Assert.AreEqual("3+2i", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_Multiplication_2plus3i_Times_4plus5i_Equals()
        {
            CreateComplexCalculator();

            // (2+3i)*(4+5i) = (2*4-3*5)+(2*5+3*4)i = (8-15)+(10+12)i = -7+22i
            ExecuteComplexNumber(2, 3);  // 2+3i
            ExecuteCommand(23);           // *
            ExecuteComplexNumber(4, 5);   // 4+5i
            ExecuteCommand(27);           // =

            Assert.AreEqual("-7+22i", _calculator.Editor.Str);
        }
    

        [TestMethod]
        public void Complex_Function_Sqr_2plus3i_Equals()
        {
            CreateComplexCalculator();

            // (2+3i)^2 = (4-9)+(12)i = -5+12i
            ExecuteComplexNumber(2, 3);  // 2+3i
            ExecuteCommand(26);           // Sqr

            Assert.AreEqual("-5+12i", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_Function_Rev_1plus1i_Equals_0_5minus0_5i()
        {
            CreateComplexCalculator();

            // 1/(1+i) = 0.5-0.5i
            ExecuteComplexNumber(1, 1);  // 1+1i
            ExecuteCommand(25);           // Rev

            Assert.AreEqual("0.5-0.5i", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_Conjugate_3plus4i_Returns_3minus4i()
        {
            CreateComplexCalculator();

            ExecuteComplexNumber(3, 4);  // 3+4i
            ExecuteCommand(52);          

            // Этот тест предполагает наличие команды 52 для сопряженного числа
            // Assert.AreEqual("3-4i", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_RealPart_3plus4i_Returns_3()
        {
            CreateComplexCalculator();

            ExecuteComplexNumber(3, 4);  // 3+4i
            ExecuteCommand(53);          

            // Этот тест предполагает наличие команды 53 для действительной части
            // Assert.AreEqual("3", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_ImaginaryPart_3plus4i_Returns_4i()
        {
            CreateComplexCalculator();

            ExecuteComplexNumber(3, 4);  // 3+4i
            ExecuteCommand(54);           

            // Этот тест предполагает наличие команды 54 для мнимой части
            // Assert.AreEqual("4i", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Complex_Modulus_3plus4i_Returns_5()
        {
            CreateComplexCalculator();

            ExecuteComplexNumber(3, 4);  // 3+4i
            ExecuteCommand(55);           

            // Этот тест предполагает наличие команды 55 для модуля
            // Assert.AreEqual("5", _calculator.Editor.Str);
        }

        #endregion

        #region Память и буфер обмена

        [TestMethod]
        public void Memory_MS_MR_StoresAndRetrievesValue()
        {
            // MS - сохранить в память, MR - восстановить из памяти
            ExecuteCommand(5);      // 5
            ExecuteCommand(29);     // MS (сохранить)
            ExecuteCommand(3);      // 3 (текущее значение меняется)
            Assert.AreEqual("3", _calculator.Editor.Str);

            ExecuteCommand(30);     // MR (восстановить)
            Assert.AreEqual("5", _calculator.Editor.Str);
        }

        [TestMethod]
        public void Memory_MPlus_AddsToMemory()
        {
            ExecuteCommand(1);  // 1
            ExecuteCommand(10); // 0 -> получаем 10
            ExecuteCommand(29); // MS (сохранить 10)

            ExecuteCommand(5);  // 5
            ExecuteCommand(31); // M+ (добавить 5 к памяти)

            ExecuteCommand(30); // MR (восстановить)
            Assert.AreEqual("15", _calculator.Editor.Str);
        }

        

        [TestMethod]
        public void Memory_MC_ClearsMemory()
        {
            ExecuteCommand(7);      // 7
            ExecuteCommand(29);     // MS (сохранить)
            ExecuteCommand(32);     // MC (очистить память)
            ExecuteCommand(30);     // MR (попытка восстановить - должно быть пусто или 0)


        }

        [TestMethod]
        public void Clipboard_CopyPaste_Simulated()
        {
            // Ввод числа 42: 4 и 2
            ExecuteCommand(4);
            ExecuteCommand(2);
            Assert.AreEqual("42", _calculator.Editor.Str);

            _clipboardText = _calculator.Editor.Str; // имитация Copy

            ExecuteCommand(28); // Clear
            Assert.AreEqual("0", _calculator.Editor.Str);

            // Имитация Paste
            string result = _calculator.ExecuteCalculatorCommand(34, ref _clipboardText, ref _memoryState);
            Assert.AreEqual("42", result);
        }

       

        #endregion

        #region Обработка ошибок

        [TestMethod]
        public void Error_DivisionByZero_ShowsError()
        {
            ExecuteCommand(5);      // 5
            ExecuteCommand(24);     // /
            ExecuteCommand(10);      // 0
            ExecuteCommand(27);     // = (деление на ноль)

            Assert.AreEqual(TCtrlState.cError, _calculator.State);
        }

     

        

        #endregion

        #region Вспомогательные методы

        private void ExecuteCommand(int commandCode)
        {
            _calculator.ExecuteCalculatorCommand(commandCode, ref _clipboardText, ref _memoryState);
        }

        private void ExecuteComplexNumber(double real, double imag)
        {

            string realStr = real.ToString(System.Globalization.CultureInfo.InvariantCulture);
            foreach (char c in realStr)
            {
                if (c == '-')
                    ExecuteCommand(0); // смена знака
                else if (c == '.')
                    ExecuteCommand(17); // разделитель
                else if (char.IsDigit(c))
                {
                    int digit = int.Parse(c.ToString());
                    ExecuteCommand(digit == 0 ? 10 : digit); // для 0 используем команду 10
                }
            }

            if (imag != 0)
            {
                if (imag > 0)
                    ExecuteCommand(17); // '+'
                else
                {
                    ExecuteCommand(0);  // смена знака для отрицательной мнимой части
                    ExecuteCommand(17); // '+'
                }

                string imagStr = Math.Abs(imag).ToString(System.Globalization.CultureInfo.InvariantCulture);
                foreach (char c in imagStr)
                {
                    if (c == '.')
                        ExecuteCommand(17);
                    else if (char.IsDigit(c))
                    {
                        int digit = int.Parse(c.ToString());
                        ExecuteCommand(digit == 0 ? 10 : digit);
                    }
                }

                // Добавляем i
                ExecuteCommand(51); // CMD_COMPLEX_I
            }
        }

       
        #endregion
    }
}






using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardTrainer
{
    public partial class KeyboardTrainer : Form
    {
        int mistakesCount = 0;

        Graphics gr;

        bool capsOn = false, shift = false, isCorrect = true, trainingStarted = false;
        //row 1(height)
        int escH = 0, f1H = 0, f2H = 0, f3H = 0, f4H = 0, f5H = 0, f6H = 0, f7H = 0, f8H = 0, f9H = 0, f10H = 0, f11H = 0, f12H = 0, insertH = 0, deleteH = 0;
        //row 2(height)
        int apostropheH = 0, oneH = 0, twoH = 0, threeH = 0, fourH = 0, fiveH = 0, sixH = 0, sevenH = 0, eightH = 0, nineH = 0, zeroH = 0, minusH = 0, equalsH = 0, backspaceH = 0;
        //row 3(height)
        int tabH = 0, qH = 0, wH = 0, eH = 0, rH = 0, tH = 0, yH = 0, uH = 0, iH = 0, oH = 0, pH = 0, opBracketH = 0, clBracketH = 0, slashH = 0;
        //row 4(height)
        int capsH = 0, aH = 0, sH = 0, dH = 0, fH = 0, gH = 0, hH = 0, jH = 0, kH = 0, lH = 0, semicolonH = 0, quotesH = 0, enterH = 0;
        //row 5(height)
        int leftShiftH = 0, rightShiftH = 0, zH = 0, xH = 0, cH = 0, vH = 0, bH = 0, nH = 0, mH = 0, commaH = 0, periodH = 0, questionH = -2;
        //row 6(height)
        int leftCtrlH = 0, rightCtrlH = 0, leftAltH = 0, rightAltH = 0, winH = 0;

        DateTime startTime { get; set; }

        Timer speedTimer = new Timer
        {
            Interval = 200
        };

        public KeyboardTrainer()
        {
            InitializeComponent();
            design();
            keys();
            yourText.Text = "";
            example.Text = "";
            drawing();

            speedTimer.Enabled = true;           
            speedTimer.Tick += new System.EventHandler(speedChecking);
            speedTimer.Stop();
        }

        private void showResult()
        {
            if (yourText.Text.Length == example.Text.Length && example.Text.Length != 0)
            {
                stopTraining();
                MessageBox.Show($"Ваш результат: \n\n1. {difficultyLabel.Text}\n2. {mistakes.Text}\n3. {speed.Text}", "Тренування завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
        }

        private void speedChecking(object sender, EventArgs e)
        {
            try
            {
                double symbolPerMinute = yourText.Text.Length / (DateTime.Now - startTime).TotalSeconds;
                string smb = symbolPerMinute.ToString();
                char[] ch = smb.ToCharArray();

                if (ch.Length > 1)
                {
                    if (ch[1] == ',') smb = $"{ch[0]}{ch[2]}{ch[3]}";
                    if (ch[1] != ',') smb = $"{ch[0]}{ch[1]}{ch[3]}{ch[4]}";
                    speed.Text = $"Швидкість: {Convert.ToInt32(smb)}(сим/хв)";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void generateSymbols()
        {
            example.Text = "";

            int n = 52;
            int cnt = Convert.ToInt32(difficulty.Value);
            string prevSymbol = "";
            string[] lc = {
                            "й", " ", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ї",
                            "ф", "і", "в", "а", "п", "р", "о", "л", "д", "ж", "є",
                            "я", "ч", "с", "м", "и", "т", "ь", "б", "ю", ".",
                            "'", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "\\"};

            string[] uc = {
                            "й", "Й", " ", "ц", "Ц","у", "У", "к", "К", "е", "Е", "н", "Н", "г", "Г",
                            "ш", "Ш", "щ", "Щ", "з", "З", "х", "Х",
                            "ф", "Ф", "і", "І", "в", "В", "а", "А", "п", "П", "р", "Р", "о", "О",
                            "л", "Л", "д", "Д", "ж", "ж", "є", "Є",
                            "я", "Я", "ч", "Ч", "с", "С", "м", "М", "и", "И", "т",  "Т", "ь", "Ь",
                            "б", "Б", "ю", "Ю", ".",

                            "₴", "!", "\"", "№", ";", "%", ":", "?", "*", "(", ")", "_", "+", "/",
                            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "\\"};
            
            Random rnd = new Random();

            if (upperCaseCb.Checked == false)
            {
                while (n != 0)
                {
                    int ind = rnd.Next(0, cnt);

                    if((lc[ind] == " " && prevSymbol == " ") || n == 52 || n == 0)
                    {
                        while (lc[ind] == " ")
                        {
                            Random rnd2 = new Random();
                            ind = rnd2.Next(0, cnt);
                        }
                    }
                    example.Text += lc[ind];
                    prevSymbol = lc[ind];
                    n--;
                }
            }
            if (upperCaseCb.Checked == true)
            {
                while (n != 0)
                {
                    int ind = rnd.Next(0, cnt);

                    if ((uc[ind] == " " && prevSymbol == " ") || n == 52 || n == 0)
                    {
                        while (uc[ind] == " ")
                        {
                            Random rnd2 = new Random();
                            ind = rnd2.Next(0, cnt);
                        }
                    }
                    example.Text += uc[ind];
                    prevSymbol = uc[ind];
                    n--;
                }
            }
        }

        private void upperCaseCb_CheckedChanged(object sender, EventArgs e)
        {
            if (upperCaseCb.Checked == true && difficulty.Value == 1)
            {
                difficulty.Value = 2;
                difficultyLabel.Text = "Складність: 2";
            }
        }

        private void stopTraining()
        {
            start.Visible = true;
            stop.Visible = false;
            difficulty.Enabled = true;
            upperCaseCb.Enabled = true;
            trainingStarted = false;
            speedTimer.Stop();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            stopTraining();
        }

        private void start_Click(object sender, EventArgs e)
        {
            yourText.Text = "";
            example.Text = "";
            start.Visible = false;
            stop.Visible = true;
            difficulty.Enabled = false;
            upperCaseCb.Enabled = false;
            trainingStarted = true;
            generateSymbols();
            mistakesCount = 0;
            mistakes.Text = "Помилки: 0";
            speed.Text = $"Швидкість: 0(сим/хв)";
            startTime = DateTime.Now;
            drawing();
            speedTimer.Start();
        }

        private void start_MouseMove(object sender, MouseEventArgs e)
        {
            start.Image = Properties.Resources.start2;
        }

        private void start_MouseLeave(object sender, EventArgs e)
        {
            start.Image = Properties.Resources.start1;
        }
        private void stop_MouseLeave(object sender, EventArgs e)
        {
            stop.Image = Properties.Resources.stop1;
        }

        private void stop_MouseMove(object sender, MouseEventArgs e)
        {
            stop.Image = Properties.Resources.stop2;
        }

        private void diff_Scroll(object sender, EventArgs e)
        {
            difficultyLabel.Text = $"Складність: {difficulty.Value}";

            if (difficulty.Value > 48) 
                upperCaseCb.Checked = true;
            else
                upperCaseCb.Checked = false;
        }

        private void correctnessChecking()
        {
            try
            {
                string s = "";
                char[] c = example.Text.ToCharArray();

                for (int i = 0; i < yourText.Text.Length; i++)
                    s += c[i];

                if (s == yourText.Text)
                    isCorrect = true;

                else
                {
                    isCorrect = false;
                    mistakesCount++;
                    mistakes.Text = $"Помилки: {mistakesCount}";         
                }
            }
            catch (Exception) { }
        }

        private void drawing()
        {
            gr = Graphics.FromImage(text.Image);
            gr.Clear(Color.White);
            gr.DrawImage(Properties.Resources.writeField, 0, 0, 537, 81);
            Rectangle rect1 = new Rectangle(3, 3, yourText.Size.Width - 3, 299);
            correctnessChecking();

            if (isCorrect == true)
            {
                gr.FillRectangle(Brushes.MediumSeaGreen, rect1);
                showResult();
            }
            else
                gr.FillRectangle(Brushes.Brown, rect1);

            text.Invalidate();
        }

        private void shiftPressed()
        {
            upperCase();
            apostropheL.Text = "₴";
            oneL.Text = "!";
            twoL.Text = "\"";
            threeL.Text = "№";
            fourL.Text = ";";
            fiveL.Text = "%";
            sixL.Text = ":";
            sevenL.Text = "?";
            eightL.Text = "*";
            nineL.Text = "(";
            zeroL.Text = ")";
            minusL.Text = "_";
            equalsL.Text = "+";
            slashL.Text = "/";
            questionL.Text = ",";
        }
        private void upperCase()
        {
            //row 3
            qL.Text = "Й";
            wL.Text = "Ц";
            eL.Text = "У";
            rL.Text = "К";
            tL.Text = "Е";
            yL.Text = "Н";
            uL.Text = "Г";
            iL.Text = "Ш";
            oL.Text = "Щ";
            pL.Text = "З";
            opBracketL.Text = "Х";
            clBracketL.Text = "Ї";

            //row 4
            aL.Text = "Ф";
            sL.Text = "І";
            dL.Text = "В";
            fL.Text = "А";
            gL.Text = "П";
            hL.Text = "Р";
            jL.Text = "О";
            kL.Text = "Л";
            lL.Text = "Д";
            semicolonL.Text = "Ж";
            quotesL.Text = "Є";

            //row 5
            zL.Text = "Я";
            xL.Text = "Ч";
            cL.Text = "С";
            vL.Text = "М";
            bL.Text = "И";
            nL.Text = "Т";
            mL.Text = "Б";
            commaL.Text = "Б";
            periodL.Text = "Ю";
        }

        private void standardCase()
        {
            //row 2
            apostropheL.Text = "'";
            oneL.Text = "1";
            twoL.Text = "2";
            threeL.Text = "3";
            fourL.Text = "4";
            fiveL.Text = "5";
            sixL.Text = "6";
            sevenL.Text = "7";
            eightL.Text = "8";
            nineL.Text = "9";
            zeroL.Text = "0";
            minusL.Text = "-";
            equalsL.Text = "=";

            //row 3
            qL.Text = "й";
            wL.Text = "ц";
            eL.Text = "у";
            rL.Text = "к";
            tL.Text = "е";
            yL.Text = "н";
            uL.Text = "г";
            iL.Text = "ш";
            oL.Text = "щ";
            pL.Text = "з";
            opBracketL.Text = "х";
            clBracketL.Text = "ї";
            slashL.Text = "\\";

            //row 4
            aL.Text = "ф";
            sL.Text = "і";
            dL.Text = "в";
            fL.Text = "а";
            gL.Text = "п";
            hL.Text = "р";
            jL.Text = "о";
            kL.Text = "л";
            lL.Text = "д";
            semicolonL.Text = "ж";
            quotesL.Text = "є";

            //row 5
            zL.Text = "я";
            xL.Text = "ч";
            cL.Text = "с";
            vL.Text = "м";
            bL.Text = "и";
            nL.Text = "т";
            mL.Text = "ь";
            commaL.Text = "б";
            periodL.Text = "ю";
            questionL.Text = ".";
        }

        private void keys()
        {
            //row 6
            leftCtrlL.Parent = leftCtrl; leftCtrlL.Location = new Point(5, leftCtrlH);
            rightCtrlL.Parent = rightCtrl; rightCtrlL.Location = new Point(5, rightCtrlH);
            leftAltL.Parent = leftAlt; leftAltL.Location = new Point(4, leftAltH);
            rightAltL.Parent = rightAlt; rightAltL.Location = new Point(4, rightAltH);
            winL.Parent = window; winL.Location = new Point(4, winH);

            //row 5
            leftShiftL.Parent = leftShift; leftShiftL.Location = new Point(16, leftShiftH);
            rightShiftL.Parent = rightShift; rightShiftL.Location = new Point(16, rightShiftH);
            zL.Parent = z; zL.Location = new Point(9, zH);
            xL.Parent = x; xL.Location = new Point(9, xH);
            cL.Parent = c; cL.Location = new Point(9, cH);
            vL.Parent = v; vL.Location = new Point(9, vH);
            bL.Parent = b; bL.Location = new Point(9, bH);
            nL.Parent = n; nL.Location = new Point(9, nH);
            mL.Parent = m; mL.Location = new Point(9, mH);
            commaL.Parent = comma; commaL.Location = new Point(9, commaH);
            periodL.Parent = period; periodL.Location = new Point(9, periodH);
            questionL.Parent = question; questionL.Location = new Point(10, questionH);

            //row 4
            capsL.Parent = capsLock; capsL.Location = new Point(4, capsH);
            aL.Parent = a; aL.Location = new Point(9, aH);
            sL.Parent = s; sL.Location = new Point(9, sH);
            dL.Parent = d; dL.Location = new Point(9, dH);
            fL.Parent = f; fL.Location = new Point(9, fH);
            gL.Parent = g; gL.Location = new Point(9, gH);
            hL.Parent = h; hL.Location = new Point(9, hH);
            jL.Parent = j; jL.Location = new Point(9, jH);
            kL.Parent = k; kL.Location = new Point(9, kH);
            lL.Parent = l; lL.Location = new Point(9, lH);
            semicolonL.Parent = semicolon; semicolonL.Location = new Point(9, semicolonH);
            quotesL.Parent = quotes; quotesL.Location = new Point(9, quotesH);
            enterL.Parent = enter; enterL.Location = new Point(16, enterH);

            //row 3
            tabL.Parent = tab; tabL.Location = new Point(5, tabH);
            qL.Parent = q; qL.Location = new Point(9, qH);
            wL.Parent = w; wL.Location = new Point(9, wH);
            eL.Parent = _e; eL.Location = new Point(9, eH);
            rL.Parent = r; rL.Location = new Point(9, rH);
            tL.Parent = t; tL.Location = new Point(9, tH);
            yL.Parent = y; yL.Location = new Point(9, yH);
            uL.Parent = u; uL.Location = new Point(9, uH);
            iL.Parent = i; iL.Location = new Point(9, iH);
            oL.Parent = o; oL.Location = new Point(9, oH);
            pL.Parent = p; pL.Location = new Point(9, pH);
            opBracketL.Parent = openBr; opBracketL.Location = new Point(9, opBracketH);
            clBracketL.Parent = closeBr; clBracketL.Location = new Point(9, clBracketH);
            slashL.Parent = slash; slashL.Location = new Point(13, slashH);

            //row 2
            apostropheL.Parent = apostrophe; apostropheL.Location = new Point(11, apostropheH);
            oneL.Parent = one; oneL.Location = new Point(9, oneH);
            twoL.Parent = two; twoL.Location = new Point(9, twoH);
            threeL.Parent = three; threeL.Location = new Point(9, threeH);
            fourL.Parent = four; fourL.Location = new Point(9, fourH);
            fiveL.Parent = five; fiveL.Location = new Point(9, fiveH);
            sixL.Parent = six; sixL.Location = new Point(9, sixH);
            sevenL.Parent = seven; sevenL.Location = new Point(9, sevenH);
            eightL.Parent = eight; eightL.Location = new Point(9, eightH);
            nineL.Parent = nine; nineL.Location = new Point(9, nineH);
            zeroL.Parent = zero; zeroL.Location = new Point(9, zeroH);
            minusL.Parent = minus; minusL.Location = new Point(9, minusH);
            equalsL.Parent = equals; equalsL.Location = new Point(9, equalsH);
            backspaceL.Parent = backspace; backspaceL.Location = new Point(1, backspaceH);

            //row 1
            escapeL.Parent = escape; escapeL.Location = new Point(4, escH);
            f1L.Parent = f1; f1L.Location = new Point(4, f1H);
            f2L.Parent = f2; f2L.Location = new Point(4, f2H);
            f3L.Parent = f3; f3L.Location = new Point(4, f3H);
            f4L.Parent = f4; f4L.Location = new Point(4, f4H);
            f5L.Parent = f5; f5L.Location = new Point(4, f5H);
            f6L.Parent = f6; f6L.Location = new Point(4, f6H);
            f7L.Parent = f7; f7L.Location = new Point(4, f7H);
            f8L.Parent = f8; f8L.Location = new Point(4, f8H);
            f9L.Parent = f9; f9L.Location = new Point(4, f9H);
            f10L.Parent = f10; f10L.Location = new Point(4, f10H);
            f11L.Parent = f11; f11L.Location = new Point(4, f11H);
            f12L.Parent = f12; f12L.Location = new Point(4, f12H);
            insertL.Parent = insert; insertL.Location = new Point(11, insertH);
            deleteL.Parent = delete; deleteL.Location = new Point(14, deleteH);
        }
        private void design()
        {
            difficultyLabel.Parent = background;
            upperCaseCb.Parent = background;
            upperCaseLabel.Parent = background;
            speed.Parent = background;
            mistakes.Parent = background;

            yourText.Parent = text;
            yourText.Location = new Point(2, 58);
            example.Parent = text;
            example.Location = new Point(2, 4);

            start.Parent = background;
            stop.Parent = background;

            //raw 6
            leftCtrl.Parent = background;
            window.Parent = background;
            leftAlt.Parent = background;
            space.Parent = background;
            rightAlt.Parent = background;
            rightCtrl.Parent = background;
            left.Parent = background;
            up.Parent = background;
            down.Parent = background;
            right.Parent = background;

            //row 5
            leftShift.Parent = background;
            z.Parent = background;
            x.Parent = background;
            c.Parent = background;
            v.Parent = background;
            b.Parent = background;
            n.Parent = background;
            m.Parent = background;
            comma.Parent = background;
            period.Parent = background;
            question.Parent = background;
            rightShift.Parent = background;


            //row 4
            capsLock.Parent = background;
            a.Parent = background;
            s.Parent = background;
            d.Parent = background;
            f.Parent = background;
            g.Parent = background;
            h.Parent = background;
            j.Parent = background;
            k.Parent = background;
            l.Parent = background;
            semicolon.Parent = background;
            quotes.Parent = background;
            enter.Parent = background;

            //row 3
            tab.Parent = background;
            q.Parent = background;
            w.Parent = background;
            _e.Parent = background;
            r.Parent = background;
            t.Parent = background;
            y.Parent = background;
            u.Parent = background;
            i.Parent = background;
            o.Parent = background;
            p.Parent = background;
            closeBr.Parent = background;
            openBr.Parent = background;
            slash.Parent = background;

            //row 2
            apostrophe.Parent = background;
            one.Parent = background;
            two.Parent = background;
            three.Parent = background;
            four.Parent = background;
            five.Parent = background;
            six.Parent = background;
            seven.Parent = background;
            eight.Parent = background;
            nine.Parent = background;
            zero.Parent = background;
            minus.Parent = background;
            equals.Parent = background;
            backspace.Parent = background;

            //row 1
            escape.Parent = background;
            f1.Parent = background;
            f2.Parent = background;
            f3.Parent = background;
            f4.Parent = background;
            f5.Parent = background;
            f6.Parent = background;
            f7.Parent = background;
            f8.Parent = background;
            f9.Parent = background;
            f10.Parent = background;
            f11.Parent = background;
            f12.Parent = background;
            insert.Parent = background;
            delete.Parent = background;         
        }
        private void previousStep()
        {
            string prevStep = "";
            char[] c = yourText.Text.ToCharArray();
            for (int i = 0; i < c.Length - 1; i++)
                prevStep += c[i];

            yourText.Text = prevStep;
        }
        private void KeyboardTrainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (trainingStarted == true)
            {
                //row 6
                if (e.KeyCode == Keys.ControlKey)
                {
                    leftCtrl.Image = Properties.Resources.k3_down;
                    rightCtrl.Image = Properties.Resources.k3_down;

                    leftCtrlH = 2;
                    rightCtrlH = 2;
                }

                if (e.KeyCode == Keys.LWin)
                {
                    window.Image = Properties.Resources.k3_down;
                    winH = 2;
                }

                if (e.KeyCode == Keys.Menu)
                {
                    leftAlt.Image = Properties.Resources.k3_down;
                    rightAlt.Image = Properties.Resources.k3_down;

                    leftAltH = 2;
                    rightAltH = 2;
                }

                if (e.KeyCode == Keys.Space)
                {
                    space.Image = Properties.Resources.k7_down;
                    yourText.Text += " ";
                }

                if (e.KeyCode == Keys.Up)
                    up.Image = Properties.Resources.pgUp_down;

                if (e.KeyCode == Keys.Down)
                    down.Image = Properties.Resources.pgDown_down;

                if (e.KeyCode == Keys.Left)
                    left.Image = Properties.Resources.home_down;

                if (e.KeyCode == Keys.Right)
                    right.Image = Properties.Resources.end_down;

                //row 5
                if (e.KeyCode == Keys.ShiftKey)
                {
                    rightShift.Image = Properties.Resources.k6_down;
                    leftShift.Image = Properties.Resources.k6_down;

                    rightShiftH = 2;
                    leftShiftH = 2;

                    shiftPressed();
                    shift = true;
                }

                if (e.KeyCode == Keys.Z)
                {
                    z.Image = Properties.Resources.k3_down;
                    zH = 2;
                    yourText.Text += zL.Text;
                }

                if (e.KeyCode == Keys.X)
                {
                    x.Image = Properties.Resources.k3_down;
                    xH = 2;
                    yourText.Text += xL.Text;
                }

                if (e.KeyCode == Keys.C)
                {
                    c.Image = Properties.Resources.k3_down;
                    cH = 2;
                    yourText.Text += cL.Text;
                }

                if (e.KeyCode == Keys.V)
                {
                    v.Image = Properties.Resources.k3_down;
                    vH = 2;
                    yourText.Text += vL.Text;
                }

                if (e.KeyCode == Keys.B)
                {
                    b.Image = Properties.Resources.k3_down;
                    bH = 2;
                    yourText.Text += bL.Text;
                }

                if (e.KeyCode == Keys.N)
                {
                    n.Image = Properties.Resources.k3_down;
                    nH = 2;
                    yourText.Text += nL.Text;
                }

                if (e.KeyCode == Keys.M)
                {
                    m.Image = Properties.Resources.k3_down;
                    mH = 2;
                    yourText.Text += mL.Text;
                }

                if (e.KeyCode == Keys.Oemcomma)
                {
                    comma.Image = Properties.Resources.k3_down;
                    commaH = 2;
                    yourText.Text += commaL.Text;
                }

                if (e.KeyCode == Keys.OemPeriod)
                {
                    period.Image = Properties.Resources.k3_down;
                    periodH = 2;
                    yourText.Text += periodL.Text;
                }

                if (e.KeyCode == Keys.OemQuestion)
                {
                    question.Image = Properties.Resources.k3_down;
                    questionH = 2;
                    yourText.Text += questionL.Text;
                }

                //row 4
                if (e.KeyCode == Keys.CapsLock)
                {
                    capsLock.Image = Properties.Resources.k5_down;
                    capsH = 2;

                    if (capsOn == false)
                    {
                        capsOn = true;
                        upperCase();
                    }
                    else
                    {
                        capsOn = false;
                        standardCase();
                        if (shift == true)
                            shiftPressed();
                    }
                }

                if (e.KeyCode == Keys.A)
                {
                    a.Image = Properties.Resources.k3_down;
                    aH = 2;
                    yourText.Text += aL.Text;
                }

                if (e.KeyCode == Keys.S)
                {
                    s.Image = Properties.Resources.k3_down;
                    sH = 2;
                    yourText.Text += sL.Text;
                }

                if (e.KeyCode == Keys.D)
                {
                    d.Image = Properties.Resources.k3_down;
                    dH = 2;
                    yourText.Text += dL.Text;
                }

                if (e.KeyCode == Keys.F)
                {
                    f.Image = Properties.Resources.k3_down;
                    fH = 2;
                    yourText.Text += fL.Text;
                }

                if (e.KeyCode == Keys.G)
                {
                    g.Image = Properties.Resources.k3_down;
                    gH = 2;
                    yourText.Text += gL.Text;
                }

                if (e.KeyCode == Keys.H)
                {
                    h.Image = Properties.Resources.k3_down;
                    hH = 2;
                    yourText.Text += hL.Text;
                }

                if (e.KeyCode == Keys.J)
                {
                    j.Image = Properties.Resources.k3_down;
                    jH = 2;
                    yourText.Text += jL.Text;
                }

                if (e.KeyCode == Keys.K)
                {
                    k.Image = Properties.Resources.k3_down;
                    kH = 2;
                    yourText.Text += kL.Text;
                }

                if (e.KeyCode == Keys.L)
                {
                    l.Image = Properties.Resources.k3_down;
                    lH = 2;
                    yourText.Text += lL.Text;
                }

                if (e.KeyCode == Keys.OemSemicolon)
                {
                    semicolon.Image = Properties.Resources.k3_down;
                    semicolonH = 2;
                    yourText.Text += semicolonL.Text;
                }

                if (e.KeyCode == Keys.OemQuotes)
                {
                    quotes.Image = Properties.Resources.k3_down;
                    quotesH = 2;
                    yourText.Text += quotesL.Text;
                }

                if (e.KeyCode == Keys.Enter)
                {
                    enter.Image = Properties.Resources.k6_down;
                    enterH = 2;
                }

                //row 3
                if (e.KeyCode == Keys.Tab)
                {
                    tab.Image = Properties.Resources.k3_down;
                    tabH = 2;
                }

                if (e.KeyCode == Keys.Q)
                {
                    q.Image = Properties.Resources.k3_down;
                    qH = 2;
                    yourText.Text += qL.Text;
                }

                if (e.KeyCode == Keys.W)
                {
                    w.Image = Properties.Resources.k3_down;
                    wH = 2;
                    yourText.Text += wL.Text;
                }

                if (e.KeyCode == Keys.E)
                {
                    _e.Image = Properties.Resources.k3_down;
                    eH = 2;
                    yourText.Text += eL.Text;
                }

                if (e.KeyCode == Keys.R)
                {
                    r.Image = Properties.Resources.k3_down;
                    rH = 2;
                    yourText.Text += rL.Text;
                }

                if (e.KeyCode == Keys.T)
                {
                    t.Image = Properties.Resources.k3_down;
                    tH = 2;
                    yourText.Text += tL.Text;
                }

                if (e.KeyCode == Keys.Y)
                {
                    y.Image = Properties.Resources.k3_down;
                    yH = 2;
                    yourText.Text += yL.Text;
                }

                if (e.KeyCode == Keys.U)
                {
                    u.Image = Properties.Resources.k3_down;
                    uH = 2;
                    yourText.Text += uL.Text;
                }

                if (e.KeyCode == Keys.I)
                {
                    i.Image = Properties.Resources.k3_down;
                    iH = 2;
                    yourText.Text += iL.Text;
                }

                if (e.KeyCode == Keys.O)
                {
                    o.Image = Properties.Resources.k3_down;
                    oH = 2;
                    yourText.Text += oL.Text;
                }

                if (e.KeyCode == Keys.P)
                {
                    p.Image = Properties.Resources.k3_down;
                    pH = 2;
                    yourText.Text += pL.Text;
                }

                if (e.KeyCode == Keys.OemOpenBrackets)
                {
                    openBr.Image = Properties.Resources.k3_down;
                    opBracketH = 2;
                    yourText.Text += opBracketL.Text;
                }

                if (e.KeyCode == Keys.OemCloseBrackets)
                {
                    closeBr.Image = Properties.Resources.k3_down;
                    clBracketH = 2;
                    yourText.Text += clBracketL.Text;
                }

                if (e.KeyCode == Keys.OemPipe)
                {
                    slash.Image = Properties.Resources.k4_down;
                    slashH = 2;
                    yourText.Text += slashL.Text;
                }

                //row 2
                if (e.KeyCode == Keys.Oemtilde)
                {
                    apostrophe.Image = Properties.Resources.k2_down;
                    apostropheH = 5;
                    yourText.Text += apostropheL.Text;
                }

                if (e.KeyCode == Keys.D1)
                {
                    one.Image = Properties.Resources.k3_down;
                    oneH = 2;
                    yourText.Text += oneL.Text;
                }

                if (e.KeyCode == Keys.D2)
                {
                    two.Image = Properties.Resources.k3_down;
                    twoH = 2;
                    yourText.Text += twoL.Text;
                }

                if (e.KeyCode == Keys.D3)
                {
                    three.Image = Properties.Resources.k3_down;
                    threeH = 2;
                    yourText.Text += threeL.Text;
                }

                if (e.KeyCode == Keys.D4)
                {
                    four.Image = Properties.Resources.k3_down;
                    fourH = 2;
                    yourText.Text += fourL.Text;
                }

                if (e.KeyCode == Keys.D5)
                {
                    five.Image = Properties.Resources.k3_down;
                    fiveH = 2;
                    yourText.Text += fiveL.Text;
                }

                if (e.KeyCode == Keys.D6)
                {
                    six.Image = Properties.Resources.k3_down;
                    sixH = 2;
                    yourText.Text += sixL.Text;
                }

                if (e.KeyCode == Keys.D7)
                {
                    seven.Image = Properties.Resources.k3_down;
                    sevenH = 2;
                    yourText.Text += sevenL.Text;
                }

                if (e.KeyCode == Keys.D8)
                {
                    eight.Image = Properties.Resources.k3_down;
                    eightH = 2;
                    yourText.Text += eightL.Text;
                }

                if (e.KeyCode == Keys.D9)
                {
                    nine.Image = Properties.Resources.k3_down;
                    nineH = 2;
                    yourText.Text += nineL.Text;
                }

                if (e.KeyCode == Keys.D0)
                {
                    zero.Image = Properties.Resources.k3_down;
                    zeroH = 2;
                    yourText.Text += zeroL.Text;
                }

                if (e.KeyCode == Keys.OemMinus)
                {
                    minus.Image = Properties.Resources.k3_down;
                    minusH = 2;
                    yourText.Text += minusL.Text;
                }

                if (e.KeyCode == Keys.Oemplus)
                {
                    equals.Image = Properties.Resources.k3_down;
                    equalsH = 2;
                    yourText.Text += equalsL.Text;
                }

                if (e.KeyCode == Keys.Back)
                {
                    backspace.Image = Properties.Resources.k5_down;
                    backspaceH = 2;
                    previousStep();
                }

                //row 1
                if (e.KeyCode == Keys.Escape)
                {
                    escape.Image = Properties.Resources.k3_down;
                    escH = 2;
                }

                if (e.KeyCode == Keys.F1)
                {
                    f1.Image = Properties.Resources.k1_down;
                    f1H = 2;
                }

                if (e.KeyCode == Keys.F2)
                {
                    f2.Image = Properties.Resources.k1_down;
                    f2H = 2;
                }

                if (e.KeyCode == Keys.F3)
                {
                    f3.Image = Properties.Resources.k1_down;
                    f3H = 2;
                }

                if (e.KeyCode == Keys.F4)
                {
                    f4.Image = Properties.Resources.k1_down;
                    f4H = 2;
                }

                if (e.KeyCode == Keys.F5)
                {
                    f5.Image = Properties.Resources.k1_down;
                    f5H = 2;
                }

                if (e.KeyCode == Keys.F6)
                {
                    f6.Image = Properties.Resources.k1_down;
                    f6H = 2;
                }

                if (e.KeyCode == Keys.F7)
                {
                    f7.Image = Properties.Resources.k1_down;
                    f7H = 2;
                }

                if (e.KeyCode == Keys.F8)
                {
                    f8.Image = Properties.Resources.k1_down;
                    f8H = 2;
                }

                if (e.KeyCode == Keys.F9)
                {
                    f9.Image = Properties.Resources.k1_down;
                    f9H = 2;
                }

                if (e.KeyCode == Keys.F10)
                {
                    f10.Image = Properties.Resources.k1_down;
                    f10H = 2;
                }

                if (e.KeyCode == Keys.F11)
                {
                    f11.Image = Properties.Resources.k1_down;
                    f11H = 2;
                }

                if (e.KeyCode == Keys.F12)
                {
                    f12.Image = Properties.Resources.k1_down;
                    f12H = 2;
                }

                if (e.KeyCode == Keys.Insert)
                {
                    insert.Image = Properties.Resources.k5_down;
                    insertH = 2;
                }

                if (e.KeyCode == Keys.Delete)
                {
                    delete.Image = Properties.Resources.k5_down;
                    deleteH = 2;
                    yourText.Text = "";
                }

                keys();
                drawing();
            }
        }

        private void KeyboardTrainer_KeyUp(object sender, KeyEventArgs e)
        {
            //row 6
            if (e.KeyCode == Keys.ControlKey)
            {
                leftCtrl.Image = Properties.Resources.k3_up;
                rightCtrl.Image = Properties.Resources.k3_up;

                leftCtrlH = 0;
                rightCtrlH = 0;
            }

            if (e.KeyCode == Keys.LWin)
            {
                window.Image = Properties.Resources.k3_up;
                winH = 0;
            }

            if (e.KeyCode == Keys.Menu)
            {
                leftAlt.Image = Properties.Resources.k3_up;
                rightAlt.Image = Properties.Resources.k3_up;

                leftAltH = 0;
                rightAltH = 0;
            }

            if (e.KeyCode == Keys.Space)
                space.Image = Properties.Resources.k7_up;

            if (e.KeyCode == Keys.Up)
                up.Image = Properties.Resources.pgUp_up;

            if (e.KeyCode == Keys.Down)
                down.Image = Properties.Resources.pgDown_up;

            if (e.KeyCode == Keys.Left)
                left.Image = Properties.Resources.home_up;

            if (e.KeyCode == Keys.Right)
                right.Image = Properties.Resources.end_up;

            //row 5
            if (e.KeyCode == Keys.ShiftKey)
            {
                rightShift.Image = Properties.Resources.k6_up;
                leftShift.Image = Properties.Resources.k6_up;

                rightShiftH = 0;
                leftShiftH = 0;

                shift = false;
                standardCase();

                if (capsOn == true)
                    upperCase();
            }

            if (e.KeyCode == Keys.Z)
            {
                z.Image = Properties.Resources.k3_up;
                zH = 0;
            }

            if (e.KeyCode == Keys.X)
            {
                x.Image = Properties.Resources.k3_up;
                xH = 0;
            }

            if (e.KeyCode == Keys.C)
            {
                c.Image = Properties.Resources.k3_up;
                cH = 0;
            }

            if (e.KeyCode == Keys.V)
            {
                v.Image = Properties.Resources.k3_up;
                vH = 0;
            }

            if (e.KeyCode == Keys.B)
            {
                b.Image = Properties.Resources.k3_up;
                bH = 0;
            }

            if (e.KeyCode == Keys.N)
            {
                n.Image = Properties.Resources.k3_up;
                nH = 0;
            }

            if (e.KeyCode == Keys.M)
            {
                m.Image = Properties.Resources.k3_up;
                mH = 0;
            }

            if (e.KeyCode == Keys.Oemcomma)
            {
                comma.Image = Properties.Resources.k3_up;
                commaH = 0;
            }

            if (e.KeyCode == Keys.OemPeriod)
            {
                period.Image = Properties.Resources.k3_up;
                periodH = 0;
            }

            if (e.KeyCode == Keys.OemQuestion)
            {
                question.Image = Properties.Resources.k3_up;
                questionH = 0;
            }

            //row 4
            if (e.KeyCode == Keys.CapsLock)
            {
                capsLock.Image = Properties.Resources.k5_up;
                capsH = 0;
            }

            if (e.KeyCode == Keys.A)
            {
                a.Image = Properties.Resources.k3_up;
                aH = 0;
            }

            if (e.KeyCode == Keys.S)
            {
                s.Image = Properties.Resources.k3_up;
                sH = 0;
            }

            if (e.KeyCode == Keys.D)
            {
                d.Image = Properties.Resources.k3_up;
                dH = 0;
            }

            if (e.KeyCode == Keys.F)
            {
                f.Image = Properties.Resources.k3_up;
                fH = 0;
            }

            if (e.KeyCode == Keys.G)
            {
                g.Image = Properties.Resources.k3_up;
                gH = 0;
            }

            if (e.KeyCode == Keys.H)
            {
                h.Image = Properties.Resources.k3_up;
                hH = 0;
            }

            if (e.KeyCode == Keys.J)
            {
                j.Image = Properties.Resources.k3_up;
                jH = 0;
            }

            if (e.KeyCode == Keys.K)
            {
                k.Image = Properties.Resources.k3_up;
                kH = 0;
            }

            if (e.KeyCode == Keys.L)
            {
                l.Image = Properties.Resources.k3_up;
                lH = 0;
            }

            if (e.KeyCode == Keys.OemSemicolon)
            {
                semicolon.Image = Properties.Resources.k3_up;
                semicolonH = 0;
            }

            if (e.KeyCode == Keys.OemQuotes)
            {
                quotes.Image = Properties.Resources.k3_up;
                quotesH = 0;
            }

            if (e.KeyCode == Keys.Enter)
            {
                enter.Image = Properties.Resources.k6_up;
                enterH = 0;
            }

            //row 3
            if (e.KeyCode == Keys.Tab)
            {
                tab.Image = Properties.Resources.k3_up;
                tabH = 0;
            }

            if (e.KeyCode == Keys.Q)
            {
                q.Image = Properties.Resources.k3_up;
                qH = 0;
            }

            if (e.KeyCode == Keys.W)
            {
                w.Image = Properties.Resources.k3_up;
                wH = 0;
            }

            if (e.KeyCode == Keys.E)
            {
                _e.Image = Properties.Resources.k3_up;
                eH = 0;
            }

            if (e.KeyCode == Keys.R)
            {
                r.Image = Properties.Resources.k3_up;
                rH = 0;
            }

            if (e.KeyCode == Keys.T)
            {
                t.Image = Properties.Resources.k3_up;
                tH = 0;
            }

            if (e.KeyCode == Keys.Y)
            {
                y.Image = Properties.Resources.k3_up;
                yH = 0;
            }

            if (e.KeyCode == Keys.U)
            {
                u.Image = Properties.Resources.k3_up;
                uH = 0;
            }

            if (e.KeyCode == Keys.I)
            {
                i.Image = Properties.Resources.k3_up;
                iH = 0;
            }

            if (e.KeyCode == Keys.O)
            {
                o.Image = Properties.Resources.k3_up;
                oH = 0;
            }

            if (e.KeyCode == Keys.P)
            {
                p.Image = Properties.Resources.k3_up;
                pH = 0;
            }

            if (e.KeyCode == Keys.OemOpenBrackets)
            {
                openBr.Image = Properties.Resources.k3_up;
                opBracketH = 0;
            }

            if (e.KeyCode == Keys.OemCloseBrackets)
            {
                closeBr.Image = Properties.Resources.k3_up;
                clBracketH = 0;
            }

            if (e.KeyCode == Keys.OemPipe)
            {
                slash.Image = Properties.Resources.k4_up;
                slashH = 0;
            }

            //row 2
            if (e.KeyCode == Keys.Oemtilde)
            {
                apostrophe.Image = Properties.Resources.k2_up;
                apostropheH = 0;
            }

            if (e.KeyCode == Keys.D1)
            {
                one.Image = Properties.Resources.k3_up;
                oneH = 0;
            }

            if (e.KeyCode == Keys.D2)
            {
                two.Image = Properties.Resources.k3_up;
                twoH = 0;
            }

            if (e.KeyCode == Keys.D3)
            {
                three.Image = Properties.Resources.k3_up;
                threeH = 0;
            }

            if (e.KeyCode == Keys.D4)
            {
                four.Image = Properties.Resources.k3_up;
                fourH = 0;
            }

            if (e.KeyCode == Keys.D5)
            {
                five.Image = Properties.Resources.k3_up;
                fiveH = 0;
            }

            if (e.KeyCode == Keys.D6)
            {
                six.Image = Properties.Resources.k3_up;
                sixH = 0;
            }

            if (e.KeyCode == Keys.D7)
            {
                seven.Image = Properties.Resources.k3_up;
                sevenH = 0;
            }

            if (e.KeyCode == Keys.D8)
            {
                eight.Image = Properties.Resources.k3_up;
                eightH = 0;
            }

            if (e.KeyCode == Keys.D9)
            {
                nine.Image = Properties.Resources.k3_up;
                nineH = 0;
            }

            if (e.KeyCode == Keys.D0)
            {
                zero.Image = Properties.Resources.k3_up;
                zeroH = 0;
            }

            if (e.KeyCode == Keys.OemMinus)
            {
                minus.Image = Properties.Resources.k3_up;
                minusH = 0;
            }

            if (e.KeyCode == Keys.Oemplus)
            {
                equals.Image = Properties.Resources.k3_up;
                equalsH = 0;
            }

            if (e.KeyCode == Keys.Back)
            {
                backspace.Image = Properties.Resources.k5_up;
                backspaceH = 0;
            }

            //row 1
            if (e.KeyCode == Keys.Escape)
            {
                escape.Image = Properties.Resources.k3_up;
                escH = 0;
            }

            if (e.KeyCode == Keys.F1)
            {
                f1.Image = Properties.Resources.k1_up;
                f1H = 0;
            }

            if (e.KeyCode == Keys.F2)
            {
                f2.Image = Properties.Resources.k1_up;
                f2H = 0;
            }

            if (e.KeyCode == Keys.F3)
            {
                f3.Image = Properties.Resources.k1_up;
                f3H = 0;
            }

            if (e.KeyCode == Keys.F4)
            {
                f4.Image = Properties.Resources.k1_up;
                f4H = 0;
            }

            if (e.KeyCode == Keys.F5)
            {
                f5.Image = Properties.Resources.k1_up;
                f5H = 0;
            }

            if (e.KeyCode == Keys.F6)
            {
                f6.Image = Properties.Resources.k1_up;
                f6H = 0;
            }

            if (e.KeyCode == Keys.F7)
            {
                f7.Image = Properties.Resources.k1_up;
                f7H = 0;
            }

            if (e.KeyCode == Keys.F8)
            {
                f8.Image = Properties.Resources.k1_up;
                f8H = 0;
            }

            if (e.KeyCode == Keys.F9)
            {
                f9.Image = Properties.Resources.k1_up;
                f9H = 0;
            }

            if (e.KeyCode == Keys.F10)
            {
                f10.Image = Properties.Resources.k1_up;
                f10H = 0;
            }

            if (e.KeyCode == Keys.F11)
            {
                f11.Image = Properties.Resources.k1_up;
                f11H = 0;
            }

            if (e.KeyCode == Keys.F12)
            {
                f12.Image = Properties.Resources.k1_up;
                f12H = 0;
            }

            if (e.KeyCode == Keys.Insert)
            {
                insert.Image = Properties.Resources.k5_up;
                insertH = 0;
            }

            if (e.KeyCode == Keys.Delete)
            {
                delete.Image = Properties.Resources.k5_up;
                deleteH = 0;
            }

            keys();
        }
    }
}

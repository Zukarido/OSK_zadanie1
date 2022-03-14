using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Zadanie1
{
    
    public partial class Form1 : Form
    {

        static class Globals
        {
            public static String instructions = null;
            public static bool complied = true;
            public static int liniakrok = 1;
        }

        public Form1()
        {
           // Console.WriteLine("siema");
            InitializeComponent();
            
        }



        private void btnWykonaj_Click(object sender, EventArgs e)
        {
            
             Globals.instructions = textBox1.Text;
                
            
            checkText(Globals.instructions);
            if (Globals.complied)
            {
                if (Globals.instructions != textBox1.Text)
                {
                    //System.Console.WriteLine("elo");
                    Globals.instructions = textBox1.Text;
                    checkText(Globals.instructions);
                    if (Globals.liniakrok <= Globals.instructions.Split('\n').Length)
                    {
                        string message = "You have changed the code druing step execution, do you wish to start execution from line 1?";
                        string caption = "Detected change of code";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;
                        result = MessageBox.Show(message, caption, buttons);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            Globals.liniakrok = 1;
                        }
                    }
                    else
                    {
                        string message = "You have changed the code druing step execution, the new code is shorter than the prievous one. The execution will proceed from the first line";
                        string caption = "Detected change of code";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result;
                        result = MessageBox.Show(message, caption, buttons);
                        Globals.liniakrok = 1;
                    }

                }

                if (!checkBox1.Checked)
                {

                    String[] instruction = Globals.instructions.Split('\n');

                    for (int i = 0; i < instruction.Length; i++)
                    {

                        interpretate(instruction[i]);
                        Globals.liniakrok++;
                        if (i == instruction.Length - 1)
                        {
                            Globals.liniakrok = 1;
                            Globals.instructions = null;

                        }
                    }

                }
                if (checkBox1.Checked)
                {
                    String[] instruction = Globals.instructions.Split('\n');
                    if (Globals.liniakrok < instruction.Length)
                    {
                        interpretate(instruction[Globals.liniakrok - 1]);
                        Globals.liniakrok++;

                    }
                    else
                    {
                        interpretate(instruction[Globals.liniakrok - 1]);
                        Globals.liniakrok = 1;
                        Globals.instructions = null;
                        // TODO: SYSTEM MESSAGE

                    }

                }
            }
            textBox1.ForeColor = Color.Blue;
        }

        private void addbinary(String tekstlabel1, String tekstlabel2, String registername)
        {
            
           
            String result=null;
            int carry = 0;
            bool overflow = false;
            int[] output = new int[8];
            for(int i=7;i>=0;i--)
            {
                int bit1 = tekstlabel1[i] - '0';
                int bit2 = tekstlabel2[i] - '0';
                output[i] = bit1 + bit2 + carry;
                if(output[i]>1)
                {
                    if (output[i] == 3)
                    {
                        output[i] = 1;
                    }
                    else
                    {
                        output[i] = 0;
                    }
                    carry = 1;
                    if (i == 0)
                    {
                        overflow = true;
                    }
                }
                else
                {
                    carry = 0;
                }

                
            }
            for(int i=0; i<8;i++)
            {
                result  += output[i].ToString();
            }
            Console.WriteLine(result);
            if (overflow)
            {
                Console.WriteLine("Overflow of the register");
            }
            else
            {
                Console.WriteLine("no Overflow");
            }
            movebinary(registername, result);
        }
        private void subbinary(String tekstlabel1, String tekstlabel2, String registername)
        {
           

            
            
            String result = null;
            int[] output = new int[8];
            bool negative = false;
            int[] bit1 = new int[8];
            int[] bit2 = new int[8];

            int a=Convert.ToInt32(tekstlabel1, 2);
            int b=Convert.ToInt32(tekstlabel2, 2);
            if (a >= b)
            {
                for(int i = 0; i < 8; i++)
                {
                     bit1[i] = tekstlabel1[i] - '0';
                     bit2[i] = tekstlabel2[i] - '0';
                }
                for (int i = 7; i >= 0; i--)
                {
                    

                    output[i] = bit1[i] - bit2[i];
                    Console.WriteLine(bit1[i]);
                    Console.WriteLine(bit2[i]);
                    if (output[i] < 0)
                    {
                        int back = carryfinder(i, bit1, 1);
                        output[i] = 1;
                        bit1[i - back] = 0;
                        for(int j = back-1; j> 0; j--)
                        {
                            bit1[i - j] = 1;
                        }

                    }
                    

                }
            }
            else
            {
                negative = true;
                for (int i = 0; i < 8; i++)
                {
                    bit2[i] = tekstlabel1[i] - '0';
                    bit1[i] = tekstlabel2[i] - '0';
                }
                for (int i = 7; i >= 0; i--)
                {


                    output[i] = bit1[i] - bit2[i];
                    Console.WriteLine(bit1[i]);
                    Console.WriteLine(bit2[i]);
                    if (output[i] < 0)
                    {
                        int back = carryfinder(i, bit1, 1);
                        output[i] = 1;
                        bit1[i - back] = 0;
                        for (int j = back - 1; j > 0; j--)
                        {
                            bit1[i - j] = 1;
                        }

                    }


                }
            }
            for(int i = 0; i < 8; i++)
            {
                result += output[i];
            }
            movebinary(registername, result);
        }
        private int carryfinder(int index, int[] bin, int back)
        {
             
            if(bin[index-1]==1)
            {
                return back;
            }
            else
            {
                back++;
                back=carryfinder(index - 1, bin, back);
                return back;
            }
           
        }
        private void movebinary(String registername, String value)
        {
            switch (registername)
            {
                case "AH":
                    lblAH.Text = value;
                    break;
                case "AL":
                    lblAL.Text = value;
                    break;
                case "BH":
                    lblBH.Text = value;
                    break;
                case "BL":
                    lblBL.Text = value;
                    break;
                case "CH":
                    lblCH.Text = value;
                    break;
                case "CL":
                    lblCL.Text = value;
                    break;
                case "DH":
                    lblDH.Text = value;
                    break;
                case "DL":
                    lblDL.Text = value;
                    break;
                
                   
            }
            
        }
        private void btnWczytaj_Click(object sender, EventArgs e)
        {

            
            
            
        }
        private void interpretate(String line)
        {
            String[] words = line.Split(' ', '\r');
            String arg1 = registergetvalue(words[1]);
            String arg2 = registergetvalue(words[2]);
            Console.WriteLine(words[0]);
            Console.WriteLine(words[1]);
            Console.WriteLine(words[2]);

            switch (words[0]){
                case "ADD":addbinary(arg1, arg2,words[1]);
                    break;
                case "SUB":
                    subbinary(arg1, arg2,words[1]);
                    break;
                case "MOVE":
                    movebinary(words[1], arg2);
                    break;

            } 
        }
        private String registergetvalue(String registername)
        {
            String result = null;
            switch (registername)
            {
                case "AH":
                    result = lblAH.Text;
                    break;
                case "AL":
                    result = lblAL.Text;
                    break;
                case "BH":
                    result = lblBH.Text;
                    break;
                case "BL":
                    result = lblBL.Text;
                    break;
                case "CH":
                    result = lblCH.Text;
                    break;
                case "CL":
                    result = lblCL.Text;
                    break;
                case "DH":
                    result = lblDH.Text;
                    break;
                case "DL":
                    result = lblDL.Text;
                    break;
                default:
                    result = registername;
                    break;
            }
            return result;

        }
        private void checkText(String text)
        {
            Globals.complied = true;
            String[] lines=text.Split('\n');
            
            for(int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(i);
                String[] words = null;
                words = lines[i].Split(' ','\r');
                if (words.Length!=3 && words.Length!=4)
                {

                    
                    Globals.complied = false;
                    break;
                }
                else
                {
                    if (words.Length == 4)
                    {
                        if (words[3] != "\r" && words[3]!="")
                        {
                            Globals.complied = false;
                            break;
                        }
                    }
                    Console.WriteLine(firstword(words[0]));
                    Console.WriteLine(secondword(words[1]));
                    Console.WriteLine(thirdword(words[2]));

                    Console.WriteLine(lines[i]);
                    if (!firstword(words[0]) || !secondword(words[1]) || !thirdword(words[2]))
                    {
                        Globals.complied = false;
                        break;
                    }
                    
                }


                
                
            }
            if (!Globals.complied)
            {
                string message = "There is a mistake in your code";
                string caption = "Instructions error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
        }
        private bool firstword(String word)
        {
            word.ToUpper();
            switch(word){
                case "ADD": return true;
                
                case "SUB":
                    return true;
                    
                case "MOVE":

                    return true;
                    
                default:
                    return false;
            }
            
        }
        private bool secondword(String word)
        {
            word.ToUpper();
            switch (word)
            {
                case "AH":
                    return true;
                case "AL":
                    return true;
                case "BH":
                    return true;
                case "BL":
                    return true;
                case "CH":
                    return true;
                case "CL":
                    return true;
                case "DH":
                    return true;
                case "DL":
                    return true;
                default:
                    return false;
            }
        }
        private bool thirdword(String word)
        {
            word.ToUpper();
            switch (word)
            {
                case "AH":
                    return true;
                case "AL":
                    return true;
                case "BH":
                    return true;
                case "BL":
                    return true;
                case "CH":
                    return true;
                case "CL":
                    return true;
                case "DH":
                    return true;
                case "DL":
                    return true;
                default:
                    if (word.Length == 8)
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (word[i] != '1' && word[i] != '0')
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    return false;
            }
        }        
    }
}



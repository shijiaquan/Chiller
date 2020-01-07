using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller
{
    public partial class ExternalTempDataOut : Form
    {
        public ExternalTempDataOut()
        {
            InitializeComponent();
        }

        BackgroundWorker Loading;
        BackgroundWorker DataOutText;

        private void DataOutClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExternalTempDataOut_Load(object sender, EventArgs e)
        {
            this.DataList.Items.Clear();

            this.TempChart.AxisX[0].Labels.Clear();
            this.TempChart.Series[0].Points.Clear();

            Loading = new BackgroundWorker();
            Loading.WorkerReportsProgress = true;           //能否报告进度更新。
            Loading.WorkerSupportsCancellation = true;      //是否支持异步取消
                                                            //绑定事件
            Loading.DoWork += new DoWorkEventHandler(Loading_DoWork);
            Loading.ProgressChanged += new ProgressChangedEventHandler(Loading_ProgressChanged);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Loading_RunWorkerCompleted);

            DataOutText = new BackgroundWorker();
            DataOutText.WorkerReportsProgress = true;           //能否报告进度更新。
            DataOutText.WorkerSupportsCancellation = true;      //是否支持异步取消
                                                                //绑定事件
            DataOutText.DoWork += new DoWorkEventHandler(DataOutText_DoWork);
            DataOutText.ProgressChanged += new ProgressChangedEventHandler(DataOutText_ProgressChanged);
            DataOutText.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DataOutText_RunWorkerCompleted);

            for (int i = 0; i < Flag.TestArm.Length; i++)
            {
                this.DataList.Items.Add(Flag.TestArm[i].Heat_Inside.Name);
            }
            for (int i = 0; i < Flag.TestArm.Length; i++)
            {
                this.DataList.Items.Add(Flag.TestArm[i].Heat_IC.Name);
            }
            for (int i = 0; i < Flag.ColdPlate.Length; i++)
            {
                this.DataList.Items.Add(Flag.ColdPlate[i].Heat.Name);
            }
            for (int i = 0; i < Flag.HotPlate.Length; i++)
            {
                this.DataList.Items.Add(Flag.HotPlate[i].Heat.Name);
            }
            for (int i = 0; i < Flag.BorderTemp.Length; i++)
            {
                this.DataList.Items.Add(Flag.BorderTemp[i].Heat.Name);
            }
        }

        private void Dataload_Click(object sender, EventArgs e)
        {
            if (this.DataList.Text == "")
            {
                MessageBox.Show(this, "未选择需要加载的数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            this.TempChart.Series[0].Title = this.DataList.Text;
            this.TempChart.AxisX[0].MaxValueLimit = int.Parse(this.NowDisplayTime.Text) * 60;
            this.LoadProgressLine.Value = 0;
            Loading.RunWorkerAsync();
        }

        private void Loading_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //句柄sender指向的就是该BackgroundWorker。

            //e.Argument 获取异步操作参数的值  
            //e.Cancel 是否应该取消事件
            //e.Result  获取或设置异步操作结果的值(在RunWorkerCompleted事件可能会使用到)
            //object a = e.Argument;//获取RunWorkerAsync(object argument)传入的值
            //BackgroundWorker worker = sender as BackgroundWorker;

            this.Invoke((MethodInvoker)delegate
            {
                string ListName = this.DataList.Text;

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    if (Flag.TestArm[i].Heat_Inside.Name == ListName)
                    {
                        this.TempChart.AxisX[0].Labels.Clear();
                        this.TempChart.Series[0].Points.Clear();
                        int NowLength = Flag.TestArm[i].Heat_Inside.NowRecord.Count;

                        if (NowLength > 0)
                        {
                            this.TimeLength.Text = (NowLength / 60f).ToString("0.0");
                            this.StartTime.Text = Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(0).Time;
                            this.EndTime.Text = Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(NowLength - 1).Time;

                            for (int j = 0; j < NowLength; j++)
                            {
                                HslControls.Charts.AxisLabel Time = new HslControls.Charts.AxisLabel();
                                HslControls.Charts.ChartPoint Point = new HslControls.Charts.ChartPoint();

                                Time.Value = j;
                                Time.Content = Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(j).Time;
                                Point.X = j;
                                Point.Y = Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(j).Value;

                                this.TempChart.AxisX[0].Labels.Add(Time);
                                this.TempChart.Series[0].Points.Add(Point);

                                Loading.ReportProgress((int)((float)j / (float)(NowLength - 1) * 100f));
                            }
                        }
                        return;
                    }
                    else if (Flag.TestArm[i].Heat_IC.Name == ListName)
                    {
                        this.TempChart.AxisX[0].Labels.Clear();
                        this.TempChart.Series[0].Points.Clear();
                        int NowLength = Flag.TestArm[i].Heat_IC.NowRecord.Count;

                        if (NowLength > 0)
                        {
                            this.TimeLength.Text = (NowLength / 60f).ToString("0.0");
                            this.StartTime.Text = Flag.TestArm[i].Heat_IC.NowRecord.GetValue(0).Time;
                            this.EndTime.Text = Flag.TestArm[i].Heat_IC.NowRecord.GetValue(NowLength - 1).Time;

                            for (int j = 0; j < NowLength; j++)
                            {
                                HslControls.Charts.AxisLabel Time = new HslControls.Charts.AxisLabel();
                                HslControls.Charts.ChartPoint Point = new HslControls.Charts.ChartPoint();

                                Time.Value = j;
                                Time.Content = Flag.TestArm[i].Heat_IC.NowRecord.GetValue(j).Time;
                                Point.X = j;
                                Point.Y = Flag.TestArm[i].Heat_IC.NowRecord.GetValue(j).Value;

                                this.TempChart.AxisX[0].Labels.Add(Time);
                                this.TempChart.Series[0].Points.Add(Point);

                                Loading.ReportProgress((int)((float)j / (float)(NowLength - 1) * 100f));
                            }
                        }
                        return;
                    }
                }

                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                {
                    if (Flag.ColdPlate[i].Heat.Name == ListName)
                    {
                        this.TempChart.AxisX[0].Labels.Clear();
                        this.TempChart.Series[0].Points.Clear();
                        int NowLength = Flag.ColdPlate[i].Heat.NowRecord.Count;

                        if (NowLength > 0)
                        {
                            this.TimeLength.Text = (NowLength / 60f).ToString("0.0");
                            this.StartTime.Text = Flag.ColdPlate[i].Heat.NowRecord.GetValue(0).Time;
                            this.EndTime.Text = Flag.ColdPlate[i].Heat.NowRecord.GetValue(NowLength - 1).Time;

                            for (int j = 0; j < NowLength; j++)
                            {
                                HslControls.Charts.AxisLabel Time = new HslControls.Charts.AxisLabel();
                                HslControls.Charts.ChartPoint Point = new HslControls.Charts.ChartPoint();

                                Time.Value = j;
                                Time.Content = Flag.ColdPlate[i].Heat.NowRecord.GetValue(j).Time;
                                Point.X = j;
                                Point.Y = Flag.ColdPlate[i].Heat.NowRecord.GetValue(j).Value;

                                this.TempChart.AxisX[0].Labels.Add(Time);
                                this.TempChart.Series[0].Points.Add(Point);

                                Loading.ReportProgress((int)((float)j / (float)(NowLength - 1) * 100f));
                            }
                        }
                        return;
                    }
                }

                for (int i = 0; i < Flag.HotPlate.Length; i++)
                {
                    if (Flag.HotPlate[i].Heat.Name == ListName)
                    {
                        this.TempChart.AxisX[0].Labels.Clear();
                        this.TempChart.Series[0].Points.Clear();
                        int NowLength = Flag.HotPlate[i].Heat.NowRecord.Count;

                        if (NowLength > 0)
                        {
                            this.TimeLength.Text = (NowLength / 60f).ToString("0.0");
                            this.StartTime.Text = Flag.HotPlate[i].Heat.NowRecord.GetValue(0).Time;
                            this.EndTime.Text = Flag.HotPlate[i].Heat.NowRecord.GetValue(NowLength - 1).Time;

                            for (int j = 0; j < NowLength; j++)
                            {
                                HslControls.Charts.AxisLabel Time = new HslControls.Charts.AxisLabel();
                                HslControls.Charts.ChartPoint Point = new HslControls.Charts.ChartPoint();

                                Time.Value = j;
                                Time.Content = Flag.HotPlate[i].Heat.NowRecord.GetValue(j).Time;
                                Point.X = j;
                                Point.Y = Flag.HotPlate[i].Heat.NowRecord.GetValue(j).Value;

                                this.TempChart.AxisX[0].Labels.Add(Time);
                                this.TempChart.Series[0].Points.Add(Point);

                                Loading.ReportProgress((int)((float)j / (float)(NowLength - 1) * 100f));
                            }
                        }
                        return;
                    }
                }

                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    if (Flag.BorderTemp[i].Heat.Name == ListName)
                    {
                        this.TempChart.AxisX[0].Labels.Clear();
                        this.TempChart.Series[0].Points.Clear();
                        int NowLength = Flag.BorderTemp[i].Heat.NowRecord.Count;

                        if (NowLength > 0)
                        {
                            this.TimeLength.Text = (NowLength / 60f).ToString("0.0");
                            this.StartTime.Text = Flag.BorderTemp[i].Heat.NowRecord.GetValue(0).Time;
                            this.EndTime.Text = Flag.BorderTemp[i].Heat.NowRecord.GetValue(NowLength - 1).Time;

                            for (int j = 0; j < NowLength; j++)
                            {
                                HslControls.Charts.AxisLabel Time = new HslControls.Charts.AxisLabel();
                                HslControls.Charts.ChartPoint Point = new HslControls.Charts.ChartPoint();

                                Time.Value = j;
                                Time.Content = Flag.BorderTemp[i].Heat.NowRecord.GetValue(j).Time;
                                Point.X = j;
                                Point.Y = Flag.BorderTemp[i].Heat.NowRecord.GetValue(j).Value;

                                this.TempChart.AxisX[0].Labels.Add(Time);
                                this.TempChart.Series[0].Points.Add(Point);

                                Loading.ReportProgress((int)((float)j / (float)(NowLength - 1) * 100f));
                            }
                        }
                        return;
                    }
                }
            });
        }

        private void Loading_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //e.Cancelled指示异步操作是否已被取消
            //e.Error 指示异步操作期间发生的错误
            //e.Result 获取异步操作结果的值,即DoWork事件中，Result设置的值。
            //if (e.Cancelled == true)
            //{
            
            //}
            //else if (e.Error != null)
            //{
            //  string a = "Error: " + e.Error.Message;
            //}
            //else
            //{
            //    string a = e.Result.ToString();
            //}
        }

        private void Loading_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            LoadProgressLine.Value = e.ProgressPercentage;
        }

        FileStream FilePath;
        private void DataOut_Click(object sender, EventArgs e)
        {
            if (this.DataList.Text == "")
            {
                MessageBox.Show(this, "未选择需要导出的数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            DateTime ST = Convert.ToDateTime(this.StartTime.Value);
            DateTime ET = Convert.ToDateTime(this.EndTime.Value);
            if (ST >= ET)
            {
                MessageBox.Show(this, "开始时间不能大于或等于结束时间！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            SaveFileDialog Save = new SaveFileDialog();

            Save.Filter = "文本(*.txt)|*.txt";
            Save.RestoreDirectory = true;
            Save.FileName = this.DataList.Text + "  " + DateTime.Now.ToString();
            DialogResult dr = Save.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK && Save.FileName.Length > 0)
            {
                FilePath = new FileStream(Save.FileName, FileMode.Create);

                DataOutText.RunWorkerAsync();

            }
            else
            {
                FilePath = null;
            }

        }

        private void DataOutText_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (FilePath != null)
            {
                StreamWriter FileWrite = new StreamWriter(FilePath);

                this.Invoke((MethodInvoker)delegate
                    {
                        string ListName = this.DataList.Text;

                        for (int i = 0; i < Flag.TestArm.Length; i++)
                        {
                            if (Flag.TestArm[i].Heat_Inside.Name == ListName)
                            {
                                int NowLength = Flag.TestArm[i].Heat_Inside.NowRecord.Count;

                                for (int j = 0; j < NowLength; j++)
                                {
                                    DateTime ST = Convert.ToDateTime(this.StartTime.Value);
                                    DateTime ET = Convert.ToDateTime(this.EndTime.Value);

                                    DateTime Now = Convert.ToDateTime(Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(j).Time);

                                    if (Now >= ST && Now <= ET)
                                    {
                                        FileWrite.Write(Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(j).Time + " " + Flag.TestArm[i].Heat_Inside.NowRecord.GetValue(j).Value + "\r\n");
                                    }
                                    else if (Now > ET)
                                    {
                                        return;
                                    }
                                }
                                return;
                            }
                            else if (Flag.TestArm[i].Heat_IC.Name == ListName)
                            {
                                int NowLength = Flag.TestArm[i].Heat_IC.NowRecord.Count;

                                for (int j = 0; j < NowLength; j++)
                                {
                                    DateTime ST = Convert.ToDateTime(this.StartTime.Value);
                                    DateTime ET = Convert.ToDateTime(this.EndTime.Value);

                                    DateTime Now = Convert.ToDateTime(Flag.TestArm[i].Heat_IC.NowRecord.GetValue(j).Time);

                                    if (Now >= ST && Now <= ET)
                                    {
                                        FileWrite.Write(Flag.TestArm[i].Heat_IC.NowRecord.GetValue(j).Time + " " + Flag.TestArm[i].Heat_IC.NowRecord.GetValue(j).Value + "\r\n");
                                    }
                                    else if (Now > ET)
                                    {
                                        return;
                                    }
                                }
                                return;
                            }
                        }

                        for (int i = 0; i < Flag.ColdPlate.Length; i++)
                        {
                            if (Flag.ColdPlate[i].Heat.Name == ListName)
                            {
                                int NowLength = Flag.ColdPlate[i].Heat.NowRecord.Count;

                                for (int j = 0; j < NowLength; j++)
                                {
                                    DateTime ST = Convert.ToDateTime(this.StartTime.Value);
                                    DateTime ET = Convert.ToDateTime(this.EndTime.Value);

                                    DateTime Now = Convert.ToDateTime(Flag.ColdPlate[i].Heat.NowRecord.GetValue(j).Time);

                                    if (Now >= ST && Now <= ET)
                                    {
                                        FileWrite.Write(Flag.ColdPlate[i].Heat.NowRecord.GetValue(j).Time + " " + Flag.ColdPlate[i].Heat.NowRecord.GetValue(j).Value + "\r\n");
                                    }
                                    else if (Now > ET)
                                    {
                                        return;
                                    }
                                }
                                return;
                            }
                        }

                        for (int i = 0; i < Flag.HotPlate.Length; i++)
                        {
                            if (Flag.HotPlate[i].Heat.Name == ListName)
                            {
                                int NowLength = Flag.HotPlate[i].Heat.NowRecord.Count;

                                for (int j = 0; j < NowLength; j++)
                                {
                                    DateTime ST = Convert.ToDateTime(this.StartTime.Value);
                                    DateTime ET = Convert.ToDateTime(this.EndTime.Value);

                                    DateTime Now = Convert.ToDateTime(Flag.HotPlate[i].Heat.NowRecord.GetValue(j).Time);

                                    if (Now >= ST && Now <= ET)
                                    {
                                        FileWrite.Write(Flag.HotPlate[i].Heat.NowRecord.GetValue(j).Time + " " + Flag.HotPlate[i].Heat.NowRecord.GetValue(j).Value + "\r\n");
                                    }
                                    else if (Now > ET)
                                    {
                                        return;
                                    }
                                }
                                return;
                            }
                        }

                        for (int i = 0; i < Flag.BorderTemp.Length; i++)
                        {
                            if (Flag.BorderTemp[i].Heat.Name == ListName)
                            {
                                int NowLength = Flag.BorderTemp[i].Heat.NowRecord.Count;

                                for (int j = 0; j < NowLength; j++)
                                {
                                    DateTime ST = Convert.ToDateTime(this.StartTime.Value);
                                    DateTime ET = Convert.ToDateTime(this.EndTime.Value);

                                    DateTime Now = Convert.ToDateTime(Flag.BorderTemp[i].Heat.NowRecord.GetValue(j).Time);

                                    if (Now >= ST && Now <= ET)
                                    {
                                        FileWrite.Write(Flag.BorderTemp[i].Heat.NowRecord.GetValue(j).Time + " " + Flag.BorderTemp[i].Heat.NowRecord.GetValue(j).Value + "\r\n");
                                    }
                                    else if (Now > ET)
                                    {
                                        return;
                                    }
                                }
                                return;
                            }
                        }
                    });

                FileWrite.Flush();          //清空缓冲区
                FileWrite.Close();        //关闭流
                FilePath.Close();
                FilePath = null;
            }
        }

        private void DataOutText_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(this, "导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DataOutText_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
           
        }

        private void In_Put_Int(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Int(sender, e);
        }
    }
}

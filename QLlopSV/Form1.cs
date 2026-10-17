using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLlopSV
{
    public partial class Form1 : Form
    {
        string str = @"Data Source=.;Initial Catalog=quanlydiem;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void grpDanhsachlop_Click(object sender, EventArgs e)
        {

        }

        private void picLop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ bảng dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE sinhvien 
                             SET tensv = @tensv, 
                                 ngaysinh = @ngaysinh, 
                                 gioitinh = @gioitinh, 
                                 sdt = @sdt, 
                                 email = @email, 
                                 malp = @malp, 
                                 trangthai = @trangthai 
                             WHERE masv = @masv";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@masv", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@tensv", txtTenSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@ngaysinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@gioitinh", cbGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@sdt", txtSDT.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@malp", cbMaLp.SelectedValue?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@trangthai", cbTrangThai.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                    ClearInput();
                    txtMaSV.Enabled = true; // Mở khóa lại ô Mã SV cho lần thêm mới tiếp theo
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxLop();
            cbTrangThai.Items.Add("Đang học");
            cbTrangThai.Items.Add("Bảo lưu");
            cbTrangThai.Items.Add("Thôi học");
            cbTrangThai.Items.Add("Đã tốt nghiệp");

            // Đặt giá trị mặc định hiển thị sẵn là "Đang học"
            cbTrangThai.SelectedIndex = 0;
        }
        


        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    string query = "SELECT masv, tensv, ngaysinh, gioitinh, sdt, email, malp, trangthai FROM sinhvien";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    guna2DataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách sinh viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadComboBoxLop()
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    // Lấy mã lớp và tên lớp từ bảng lop
                    string query = "SELECT malp, tenlop FROM lop";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Thêm dòng mặc định hiển thị "Tất cả lớp" ở đầu tiên
                    DataRow dr = dt.NewRow();
                    dr["malp"] = "";
                    dr["tenlop"] = "-- Tất cả lớp --";
                    dt.Rows.InsertAt(dr, 0);

                    cbMaLp.DataSource = dt;
                    cbMaLp.DisplayMember = "tenlop"; // Hiển thị tên lớp cho người dùng thấy
                    cbMaLp.ValueMember = "malp";     // Giá trị ngầm bên trong là mã lớp (malp)
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu ComboBox lớp: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
 

                string tuKhoa = txtTimkiem.Text.Trim();
                string maLopChon = cboTrangThai.SelectedValue?.ToString();

                using (SqlConnection conn = new SqlConnection(str))
                {
                    // Câu lệnh truy vấn tìm kiếm theo Mã/Tên sinh viên kết hợp lọc theo Mã Lớp (malp)
                    string query = @"SELECT masv, tensv, ngaysinh, gioitinh, sdt, email, malp, trangthai 
                                 FROM sinhvien 
                                 WHERE (masv LIKE @keyword OR tensv LIKE @keyword) ";

                    // Nếu người dùng chọn một lớp cụ thể (mã lớp khác rỗng) thì cộng thêm điều kiện lọc
                    if (!string.IsNullOrEmpty(maLopChon))
                    {
                        query += " AND malp = @malp";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + tuKhoa + "%");
                    if (!string.IsNullOrEmpty(maLopChon))
                    {
                        cmd.Parameters.AddWithValue("@malp", maLopChon);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    guna2DataGridView1.DataSource = dt;
                }
            }
        private void ClearInput()
        {
            txtMaSV.Clear();
            txtTenSV.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtMaSV.Enabled = true; // Mở khóa mã sinh viên để có thể nhập mã mới
        }
        private void btnThemSinhVien_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtMaSV.Text) || string.IsNullOrEmpty(txtTenSV.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO sinhvien (masv, tensv, ngaysinh, gioitinh, sdt, email, malp, trangthai) 
                             VALUES (@masv, @tensv, @ngaysinh, @gioitinh, @sdt, @email, @malp, @trangthai)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@masv", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@tensv", txtTenSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@ngaysinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@gioitinh", cbGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@sdt", txtSDT.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@malp", cbMaLp.SelectedValue?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@trangthai", cbTrangThai.Text);
                  
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thêm sinh viên thành công! (Hệ thống đã tự động tạo tài khoản đăng nhập cho sinh viên)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData(); // Làm mới lại bảng DataGridView
                    ClearInput();           // Xóa trắng ô nhập liệu
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên (Có thể trùng mã SV hoặc email): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Gán dữ liệu từ các cột lên các control nhập liệu tương ứng của bạn
                // (Thay thế tên các control text/datetimepicker cho phù hợp với form của bạn)
                txtMaSV.Text = row.Cells["masv"].Value?.ToString();
                txtTenSV.Text = row.Cells["tensv"].Value?.ToString();

                if (row.Cells["ngaysinh"].Value != DBNull.Value && row.Cells["ngaysinh"].Value != null)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["ngaysinh"].Value);
                }

                cbGioiTinh.Text = row.Cells["gioitinh"].Value?.ToString();
                txtSDT.Text = row.Cells["sdt"].Value?.ToString();
                txtEmail.Text = row.Cells["email"].Value?.ToString();
                cbMaLp.SelectedValue = row.Cells["malp"]?.Value?.ToString() ?? "";
                cbTrangThai.Text = row.Cells["trangthai"].Value?.ToString();

                // Khi đang chọn để sửa, khóa ô Mã SV lại để tránh bị sửa khóa chính
                txtMaSV.Enabled = false;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string masv = guna2DataGridView1.SelectedRows[0].Cells["masv"].Value.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có mã '{masv}' không?\n(Tài khoản hệ thống liên quan cũng sẽ bị xóa theo)", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(str))
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM sinhvien WHERE masv = @masv";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@masv", masv);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadData();
                            ClearInput();
                            txtMaSV.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa do sinh viên này đã có dữ liệu điểm số hoặc phúc khảo liên quan!", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng sinh viên cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblGioiTinh_Click(object sender, EventArgs e)
        {

        }

        private void lblMaLop_Click(object sender, EventArgs e)
        {

        }
    }
    }


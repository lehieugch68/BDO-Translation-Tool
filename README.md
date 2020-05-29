# BDO Translation Tool
Công cụ hỗ trợ dịch trò chơi Black Desert Online.

### Chức năng: 

* Giải nén/giải mã, chuyển đổi định dạng tệp languagedata_en.loc về dạng TSV để chỉnh sửa ngôn ngữ trong game Black Desert Online.
* Hỗ trợ cập nhật bản dịch vào file mới khi game update.
* Tải bản dịch của người khác và gộp bản dịch.

### Cách dùng:

* Chọn đường dẫn đến thư mục game.
* Bấm **Giải nén** để giải nén/giải mã file **languagedate_en.loc** về dạng TSV. Do khối lượng text khá lớn (hơn 700.000 dòng) nên sẽ mất khá nhiều thời gian, ai máy yếu có thể tải file được giải nén sẵn ở đây (file được game cập nhật ngày 27/5/2020): [BDO_Translation.tsv](https://drive.google.com/file/d/1UhdQMK2A0kpfHq_oUZdTmX0GkwPZm3JB/view?usp=sharing)
* Dùng **Excel** để chỉnh sửa (dịch) file **BDO_Translation.tsv**.
* Bấm **Cài đặt** để sao chép bản dịch (ở file **BDO_Translation.tsv**) vào file gốc của game.
* Mỗi khi game cập nhật bấm **Sao lưu** trước rồi **Cài đặt** để cập nhật bản dịch vào file mới.

### Lưu ý: 
* Không sửa cột tiếng Anh gốc.
* Chỉ nên giải nén một lần vì thời gian lọc câu trùng khá lâu, trừ khi bạn đã dịch hết toàn bộ game và muốn dịch tiếp các nội dung mới cập nhật.
* Luôn giữ format gốc của file để tránh bị lỗi: định dạng **TSV**, mã hóa **UCS-2 LE BOM** (UTF-16).
* Chỉ sửa file **BDO_Translation.tsv**, không chỉnh sửa các file khác.
* Muốn xuống dòng trong câu dùng ký tự \<lf>.
* Tất cả ký tự nháy kép (") thay thế bằng **\<quot>** để tránh lỗi với Excel.
* Các câu dịch có ký tự **+, =, -** ở đầu thì thêm dấu nháy đơn (') vào trước để tránh Excel hiểu nhầm là phép tính gây ra lỗi.
* Do khối lượng text quá lớn (hơn 700k dòng) nên tool sẽ loại bỏ đi những câu bị trùng. Đối với ai muốn dịch chính xác ngữ cảnh (theo ID) thì tham khảo tool này: [BDO LanguageData Tool](https://github.com/lehieugch68/BDO-LanguageData-Tool).

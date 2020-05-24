# BDO Translation Tool
Công cụ hỗ trợ dịch trò chơi Black Desert Online.

### Chức năng: 

* Giải nén/nén, chuyển đổi định dạng tệp languagedata_en.loc về dạng TSV để chỉnh sửa ngôn ngữ trong game Black Desert Online.
* Hỗ trợ cập nhật bản dịch vào tệp mới khi game update.

### Cách dùng:

* Sao lưu file languagedata_en.loc trong thư mục ads của game.
* Chọn đường dẫn đến thư mục game.
* Bấm **Giải nén** để giải nén/giải mã file **languagedate_en.loc** về dạng TSV. Do khối lượng text quá lớn (hơn 700k dòng) nên tool sẽ loại bỏ đi những câu bị trùng. Đối với ai muốn dịch chính xác ngữ cảnh (theo ID) thì tham khảo tool này: [BDO LanguageData Tool](https://github.com/lehieugch68/BDO-LanguageData-Tool).
* Dùng **Excel** để chỉnh sửa (dịch) file **BDO_Translation.tsv**, luôn giữ format gốc của file để tránh bị lỗi: định dạng **TSV**, mã hóa **UCS-2 LE BOM** (UTF-16). Không chỉnh sửa các file khác.
* Bấm **Cài đặt** để sao chép bản dịch (ở file **BDO_Translation.tsv**) vào file gốc của game. Mỗi khi game cập nhật chỉ cần bấm **Cài đặt** để cập nhật bản dịch vào file mới.
* Lưu ý: Chỉ nên giải nén một lần vì thời gian lọc câu trùng khá lâu, trừ khi bạn đã dịch hết toàn bộ game và muốn dịch tiếp các nội dung mới cập nhật.

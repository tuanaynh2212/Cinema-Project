-- Chạy trong Supabase SQL Editor trước khi chạy bản code mới.
-- user_id lấy từ auth.users.id (UUID), được gửi bằng JWT của người dùng đã đăng nhập.

alter table public.ghe_suat_chieu
    add column if not exists user_id uuid references auth.users(id) on delete set null;

create index if not exists idx_ghe_suat_chieu_user_id
    on public.ghe_suat_chieu(user_id);

-- LƯU Ý RLS:
-- 1) INSERT phải giới hạn: with check (auth.uid() = user_id).
-- 2) SELECT cần cho người dùng đã xác thực đọc ghế đã đặt của mọi tài khoản
--    để sơ đồ ghế không cho hai người đặt trùng; màn hình 'Vé của bạn' vẫn lọc user_id.
-- 3) Kiểm tra/điều chỉnh các policy INSERT cũ nếu đang cho phép with check (true).
-- Không xóa policy hiện có tự động vì project của bạn đã được phân quyền trước đó.

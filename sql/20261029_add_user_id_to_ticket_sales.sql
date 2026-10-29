-- Bổ sung user_id cho ticket_sales để gắn mỗi giao dịch với tài khoản Supabase.
-- Chạy trong Supabase Dashboard -> SQL Editor.
-- Cột để NULL nhằm giữ nguyên các giao dịch cũ chưa xác định được chủ sở hữu.

alter table public.ticket_sales
    add column if not exists user_id uuid
    references auth.users(id) on delete set null;

create index if not exists idx_ticket_sales_user_id
    on public.ticket_sales(user_id);

-- RLS cần được rà soát theo vai trò hiện có:
-- Người dùng thường chỉ SELECT hàng có user_id = auth.uid().
-- Admin có thể SELECT toàn bộ giao dịch theo policy admin hiện có.
-- INSERT nên dùng WITH CHECK (auth.uid() = user_id).
-- Không tạo policy rộng WITH CHECK (true).
-- Không tự gán user_id cho các hàng cũ: ảnh hiện tại không cho biết chủ sở hữu các giao dịch đó.

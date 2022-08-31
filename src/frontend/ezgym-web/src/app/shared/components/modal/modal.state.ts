export interface ModalState {
  title: string,
  description: string,
  confirmButtonLabel?: string,
  opened?: boolean,
  fixed?: boolean,
  iconSrc?: string
  onConfirm: () => void
}
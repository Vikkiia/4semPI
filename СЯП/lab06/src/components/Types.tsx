export type Operator = "+" | "-" | "*" | "/";
export type Theme = "light" | "dark";

export interface CalculatorState {
  input: string;
  result: string;
  history: string[];
}

export interface ButtonProps {
  label: string;
  onClick: (value: string) => void;
  className?: string;
}

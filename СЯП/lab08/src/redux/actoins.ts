import { INCREMENT, DECREMENT, RESET, CounterActionTypes } from './types';
//action
export const increment = (): CounterActionTypes => ({ type: INCREMENT });
export const decrement = (): CounterActionTypes => ({ type: DECREMENT });
export const reset = (): CounterActionTypes => ({ type: RESET });

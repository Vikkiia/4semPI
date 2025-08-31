import { INCREMENT, DECREMENT, RESET, CounterActionTypes } from './types';

interface CounterState {
    count: number;
}

const initialState: CounterState = {
    count: 0,
};

export const counterReducer = (state = initialState, action: CounterActionTypes): CounterState => {
    switch (action.type) {
        case INCREMENT:
            return { count: state.count + 1 };
        case DECREMENT:
            return { count: state.count - 1 };
        case RESET:
            return { count: 0 };
        default:
            return state;
    }
};

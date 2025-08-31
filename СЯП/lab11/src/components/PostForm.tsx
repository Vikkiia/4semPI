import React, { useState, useEffect } from 'react';
import { useAppDispatch } from '../hooks';
import { createPost, updatePost } from '../features/posts/postsSlice';
import { Post, NewPost } from '../features/posts/postsAPI';

interface Props {
    editingPost: Post | null;
    clearEditing: () => void;
}

export const PostForm: React.FC<Props> = ({ editingPost, clearEditing }) => {
    const dispatch = useAppDispatch();
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');

    useEffect(() => {
        if (editingPost) {
            setTitle(editingPost.title);
            setBody(editingPost.body);
        }
    }, [editingPost]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (editingPost) {
            dispatch(updatePost({ ...editingPost, title, body }));
        } else {
            const newPost: NewPost = { title, body };
            dispatch(createPost(newPost));
        }
        setTitle('');
        setBody('');
        clearEditing();
    };

    return (
        <form onSubmit={handleSubmit} className="mb-4">
            <input className="border w-full p-2 mb-2" value={title} onChange={e => setTitle(e.target.value)} placeholder="Title" required />
            <textarea className="border w-full p-2 mb-2" value={body} onChange={e => setBody(e.target.value)} placeholder="Body" required />
            <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
                {editingPost ? 'Update Post' : 'Add Post'}
            </button>
        </form>
    );
};

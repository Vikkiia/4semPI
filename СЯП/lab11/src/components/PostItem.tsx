import React from 'react';
import { useAppDispatch } from '../hooks';
import { deletePost } from '../features/posts/postsSlice';
import { Post } from '../features/posts/postsAPI';

interface Props {
    post: Post;
    onEdit: (post: Post) => void;
}

export const PostItem: React.FC<Props> = ({ post, onEdit }) => {
    const dispatch = useAppDispatch();
    return (
        <div className="border p-2 mb-2">
            <h3 className="font-bold">{post.title}</h3>
            <p>{post.body}</p>
            <div className="mt-2 flex gap-2">
                <button onClick={() => onEdit(post)} className="bg-yellow-500 text-white px-2 py-1 rounded">Edit</button>
                <button onClick={() => dispatch(deletePost(post.id))} className="bg-red-500 text-white px-2 py-1 rounded">Delete</button>
            </div>
        </div>
    );
};
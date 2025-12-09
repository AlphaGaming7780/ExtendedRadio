import { getModule } from "cs2/modding";
import { MouseEventHandler } from "react";

// Thanks krzychu124 for that.
export interface DragEventData { clientX: number, clientY: number, currentTarget: HTMLElement }

const useMouseDragEventsModule = getModule('game-ui/common/hooks/use-mouse-drag-events.tsx', 'useMouseDragEvents')

export interface UseMouseDragEventsProps {
    handleDragStart?: (e: MouseEvent) => boolean,
    handleDragging?: (a: DragEventData) => void,
    handleDragEnd?: (a: DragEventData) => void,
}

export function useMouseDragEvents({ handleDragStart = () => true, handleDragging = () => { }, handleDragEnd = () => { } } : UseMouseDragEventsProps): { isDragging: boolean, handleMouseDown: MouseEventHandler } {
    const [a, b] = useMouseDragEventsModule(handleDragStart, handleDragging, handleDragEnd)
    const isDragging = a as boolean
    const handleMouseDown = b as MouseEventHandler
    return { isDragging, handleMouseDown }
}

//export const useMouseDragEvents: { isDragging: any, handleMouseDown: any } = (useMouseDragEventsProps : UseMouseDragEventsProps) => {
//    const [a, b] = useMouseDragEventsModule(useMouseDragEventsProps)

//    const isDragging = a as any
//    const handleMouseDown = b as any

//    return { isDragging, handleMouseDown }
//}
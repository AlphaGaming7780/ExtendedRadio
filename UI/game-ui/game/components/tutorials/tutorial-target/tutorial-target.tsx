import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/tutorials/tutorial-target/tutorial-target.tsx"

export type PropsTutorialTarget = {
    uiTag: string | undefined,
    active?: any,
    disableBlinking?: any,
    children: any
}

export function TutorialTarget(propsTutorialTarget: PropsTutorialTarget): JSX.Element {
    return getModule(path$, "TutorialTarget")(propsTutorialTarget)
}

import React, { ReactElement } from "react"
import { getModule } from "cs2/modding"

const PATH = "game-ui/game/components/tutorials/tutorial-target/tutorial-target.tsx"

export type TutorialTargetProps = {
    uiTag?: string;
    active?: boolean;
    disableBlinking?: boolean;
    editor?: boolean;
    children: React.ReactElement;
};

export const TutorialTarget = getModule(
    PATH,
    "TutorialTarget"
) as React.ForwardRefExoticComponent<
    TutorialTargetProps & React.RefAttributes<HTMLElement>
>;
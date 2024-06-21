import { getModule } from "cs2/modding"

const path$ = "game-ui/common/text/formatted-paragraphs.module.scss"

export const FormattedParagraphsSCSS = {
	paragraphs: getModule(path$, "classes").paragraphs,
}
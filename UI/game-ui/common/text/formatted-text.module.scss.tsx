import { getModule } from "cs2/modding"

const path$ = "game-ui/common/text/formatted-text.module.scss"

export const FormattedTextSCSS = {
	h1: getModule(path$, "classes").h1,
	h2: getModule(path$, "classes").h2,
	h3: getModule(path$, "classes").h3,
	h4: getModule(path$, "classes").h4,
	h5: getModule(path$, "classes").h5,
	h6: getModule(path$, "classes").h6,
	link: getModule(path$, "classes").link,
	p: getModule(path$, "classes").p,
	listItem: getModule(path$, "classes").listItem,
}